using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Needle.Engine.Core;
using Needle.Engine.Editors;
using Needle.Engine.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Needle.Engine.ProjectBundle
{
	public static class BundleExportInfoEditorExtension
	{
		[InitializeOnLoadMethod]
		private static void Init()
		{
			if (!wrapper)
				wrapper = ScriptableObject.CreateInstance<NpmDefDependenciesWrapper>();
			wrapper.Dependencies.Clear();
			
			ExportInfoEditor.Enabled += OnEnabled;
			ExportInfoEditor.LateInspectorGUI += OnGUI;
			// We need to update the dependencies before the build starts
			// Because installation happens *after* the build but codegen happens during the build + checking which scripts are used
			Builder.BuildStarting += () =>
			{
				UpdateDependencies(ExportInfo.Get());
			};
		}

		private static NpmDefDependenciesWrapper wrapper = null;
		private static SerializedObject serializedObject;
		private static SerializedProperty property;
		private static readonly List<NpmDefObject> previouslyAssignedDependencies = new List<NpmDefObject>();

		private static void OnEnabled(ExportInfo obj)
		{
			if (!wrapper)
				wrapper = ScriptableObject.CreateInstance<NpmDefDependenciesWrapper>();
			wrapper.Dependencies.Clear();

			// Make sure every dependency also has a guid
			for (var index = 0; index < obj.Dependencies.Count; index++)
			{
				var dep = obj.Dependencies[index];
				if (string.IsNullOrEmpty(dep.Guid) && File.Exists(dep.VersionOrPath))
				{
					var guid = AssetDatabase.AssetPathToGUID(dep.VersionOrPath);
					Debug.Log("Added missing guid to serialized dependencies: " + dep.Name + "@" + dep.VersionOrPath + " → " + guid);
					dep.Guid = guid;
					obj.Dependencies[index] = dep;
				}
			}
			
			UpdateDependencies(obj);

			previouslyAssignedDependencies.Clear();
			previouslyAssignedDependencies.AddRange(wrapper.Dependencies);

			serializedObject = new SerializedObject(wrapper);
			property = serializedObject.FindProperty(nameof(NpmDefDependenciesWrapper.Dependencies));
		}

		private static void UpdateDependencies(ExportInfo obj)
		{
			var projectDirectory = Path.GetFullPath(obj.GetProjectDirectory());
			var shouldValidatePackageJsonDependencies = false;

			// Check that dependencies serialized in ExportInfo have the correct name assigned
			// this happens if we e.g. rename an npmdef
			var previouslyFoundDependencies = new HashSet<(string,string,string)>();
			for (var index = obj.Dependencies.Count - 1; index >= 0; index--)
			{
				var dep = obj.Dependencies[index];
				var directory = dep.VersionOrPath.Replace(".npmdef", "~");
				var fullPath = Path.GetFullPath(directory + "/package.json");
				if (File.Exists(fullPath))
				{
					var name = PackageUtils.GetPackageName(fullPath);
					if (string.IsNullOrEmpty(name))
					{
						continue;
					}
					if (dep.Name != name)
					{
						dep.Name = name;
						Debug.Log($"Update serialized dependency with wrong name. Serialized \"{dep.Name}\" is actually \"{name}\"".LowContrast());
						obj.Dependencies[index] = dep;
						shouldValidatePackageJsonDependencies = true;
					}
					
					// Make sure that we dont keep the same serialized dependency multiple times in our list
					var key = (dep.Name, dep.VersionOrPath, dep.Guid);
					if (previouslyFoundDependencies.Contains(key))
					{
						obj.Dependencies.RemoveAt(index);
						shouldValidatePackageJsonDependencies = true;
						continue;
					}
					previouslyFoundDependencies.Add(key);
				}
			}
			
			const string packageJsonDependenciesKey = "dependencies";
			if (PackageUtils.TryReadDependencies(obj.PackageJsonPath, out var deps, packageJsonDependenciesKey))
			{
				// make sure serialized dependencies get installed
				var didInstall = false;
				foreach (var dep in obj.Dependencies)
				{
					if (string.IsNullOrWhiteSpace(dep.Name))
					{
						Debug.LogWarning("Found serialized dependency without name. This should not happen. Is the package missing?: " + dep.VersionOrPath + " → " + dep.Guid);
						continue;
					}
					if (!deps.ContainsKey(dep.Name))
					{
						dep.Install(obj.PackageJsonPath);
						didInstall = true;
					}
				}
				if (didInstall)
				{
					TypesUtils.MarkDirty();
					PackageUtils.TryReadDependencies(obj.PackageJsonPath, out deps);
				}
				
				var dependenciesToRemove = new List<string>();

				for (var index = 0; index < BundleRegistry.Instance.Bundles.Count; index++)
				{
					var bundle = BundleRegistry.Instance.Bundles[index];
					var packageName = bundle.FindPackageName();
					if (packageName != null && IsNpmdefInDependencies(projectDirectory, deps, packageName, bundle.PackageDirectory))
					{
						// If the package contains npmdef dependencies that are not serialized in the ExportInfo component
						// then we want to remove them from the temporary project (instead of adding or displaying them in ExportInfo)
						if (obj.IsTempProject() && !obj.Dependencies.Any(d => d.Name == packageName))
						{
							dependenciesToRemove.Add(packageName);
							continue;
						}

						var path = bundle.FilePath;
						var def = AssetDatabase.LoadAssetAtPath<NpmDefObject>(path);
						if (def)
						{
							def.displayName = packageName;
							wrapper.Dependencies.Add(def);
							// add the npmdef to the ExportInfo (but dont modify it implicitly for temp projects)
							if (obj.IsTempProject() == false && !obj.Dependencies.Any(d => d.Name == packageName))
								TryAddDependenciesToSerializedList(obj.Dependencies, def);
						}
					}
				}

				if (dependenciesToRemove.Count > 0)
				{
					foreach (var dep in dependenciesToRemove)
					{
						deps.Remove(dep);
					}
					PackageUtils.TryWriteDependencies(obj.PackageJsonPath, deps, packageJsonDependenciesKey);
				}

				if (shouldValidatePackageJsonDependencies)
				{
					EnsureThatNpmdefPackageDependenciesExistOnlyOnce(projectDirectory, packageJsonDependenciesKey, deps);
				}
			}

		}

		/// <summary>
		/// Iterates over package json dependencies and check if a dependency is a npmdef
		/// If the name (key) and the dependency name (npmdef package.json name) dont match we remove the dependency from our web project
		/// </summary>
		/// <param name="projectDirectory">web project dir</param>
		/// <param name="dependenciesKey">web project package.json KEY (e.g. "dependencies" or "devDependencies")</param>
		/// <param name="dependencies">the dependencies of the package.json</param>
		private static void EnsureThatNpmdefPackageDependenciesExistOnlyOnce(string projectDirectory, string dependenciesKey, Dictionary<string, string> dependencies)
		{
			var toRemove = new List<string>();
			var knownNpmdefBundles = BundleRegistry.Instance.Bundles;
			foreach (var dep in dependencies)
			{
				if (PackageUtils.TryGetPath(projectDirectory, dep.Value, out var path))
				{
					var packageJsonPath = Path.Combine(path, "package.json");
					if (File.Exists(packageJsonPath))
					{
						var name = PackageUtils.GetPackageName(packageJsonPath);
						if (name != dep.Key  && knownNpmdefBundles.Any(b => b.PackageDirectory == path))
						{
							toRemove.Add(dep.Key);
						}
					}
				}
			}
			if (toRemove.Count > 0)
			{
				foreach(var key in toRemove)
				{
					Debug.Log($"Removing dependency that does not exist anymore: {toRemove}".LowContrast());
					dependencies.Remove(key);
				}
				PackageUtils.TryWriteDependencies(projectDirectory + "/package.json", dependencies, dependenciesKey);
			}
		}

		private static bool IsNpmdefInDependencies(string projectDirectory,
			Dictionary<string, string> packageJson,
			string npmdefName,
			string npmdefPath)
		{
			if (packageJson.ContainsKey(npmdefName)) return true;
			foreach (var dep in packageJson)
			{
				if (dep.Key == npmdefName) return true;
				// Sometimes the path variables were not fully resolved e.g. something like <threejs> 
				if (dep.Value.Contains("<") && dep.Value.Contains(">"))
				{
					continue;
				}
				var path = Path.GetFullPath(projectDirectory + "/" + dep.Value);
				if (path == npmdefPath) return true;
			}
			return false;
		}

		private static void OnGUI(ExportInfo obj)
		{
			if (!obj) return;

			if (!obj.Exists() || !obj.IsValidDirectory())
			{
				RenderMissingDependencies(obj);
				return;
			}
			if (property == null) return;
			// using (new EditorGUI.DisabledScope(obj.IsTempProject()))
			{
				using (var change = new EditorGUI.ChangeCheckScope())
				{
					GUILayout.Space(5);
					var changed = EditorGUILayout.PropertyField(property,
						new GUIContent($"Dependencies ({wrapper.Dependencies.Count})",
							"Dependencies in your packages.json that are found as NpmDef files in your project!.\n\nUse the context menu in the Project browser to create new NpmDef packages to better organize your web project."), true);
					if (changed || change.changed || wrapper.Dependencies.Count != previouslyAssignedDependencies.Count)
					{
						serializedObject.ApplyModifiedProperties();
						HandleDependenciesChanged(obj);
					}
				}
			}
			RenderMissingDependencies(obj);
		}

		private static void HandleDependenciesChanged(ExportInfo exp)
		{
			if (!PackageUtils.TryReadDependencies(exp.PackageJsonPath, out var dependencies)) return;

			var serializedDependencies = exp.Dependencies;

			var didChange = false;

			var current = wrapper.Dependencies;
			foreach (var dep in previouslyAssignedDependencies)
			{
				if (!dep) continue;
				if (!current.Contains(dep))
				{
					var bundle = dep.FindBundle();
					var packageName = bundle?.FindPackageName();
					if (packageName != null && dependencies.ContainsKey(packageName))
					{
						Debug.Log("<b>Remove dependency</b> to " + packageName + " from " + exp.DirectoryName + "\nin package " + exp.PackageJsonPath);
						didChange = true;
						dependencies.Remove(packageName);
						serializedDependencies.RemoveAll(d => d.Name == packageName);

						// remove the workspace path entries in the current project
						var dir = exp.GetProjectDirectory();
						Actions.RemoveFromWorkspace(dir, packageName);
					}
				}
			}

			var toInstall = new List<Bundle>();
			foreach (var cur in current)
			{
				if (!cur) continue;
				var bundle = cur.FindBundle();
				if (bundle == null) continue;
				bundle.FilePath = AssetDatabase.GetAssetPath(cur);
				if (!previouslyAssignedDependencies.Contains(cur))
				{
					var packageName = bundle.FindPackageName();
					if (packageName != null)
					{
						if (!dependencies.ContainsKey(packageName))
						{
							Debug.Log("<b>Add dependency</b> to " + packageName + " to " + exp.DirectoryName + "\nin package " + exp.PackageJsonPath);
							didChange = true;
							var dir = bundle.PackageDirectory;
							var target = Path.GetFullPath(exp.GetProjectDirectory());
							var path = PackageUtils.GetFilePath(target, dir);
							dependencies.Add(packageName, path);
							Actions.AddToWorkspace(target, packageName);
							toInstall.Add(bundle);
						}
					}
					else Debug.LogWarning("Could not find package name in " + bundle.PackageFilePath.AsLink());
				}

				TryAddDependenciesToSerializedList(serializedDependencies, cur);
			}

			previouslyAssignedDependencies.Clear();
			previouslyAssignedDependencies.AddRange(current);

			if (didChange)
			{
				PackageUtils.TryWriteDependencies(exp.PackageJsonPath, dependencies);
				TypesUtils.MarkDirty();
				RunInstallation(toInstall);
			}
		}

		private static async void RunInstallation(IList<Bundle> bundles)
		{
			if(bundles.Count > 0) Debug.Log("Installing " + bundles.Count + " dependencies...");
			foreach(var bundle in bundles)
				await bundle.RunInstall();
			OnDependencyAdded();
		}

		private static int _dependencyAddedId;
		private static async void OnDependencyAdded()
		{
			var id = ++_dependencyAddedId;
			await Task.Delay(2500);
			if(id != _dependencyAddedId) return;
			if (Engine.Actions.IsInstalling()) return;
			ViteActions.DeleteCache();
			TypesUtils.MarkDirty();
			await Engine.Actions.InstallPackage(false, false);
		}

		private static bool TryAddDependenciesToSerializedList(List<Dependency> dependencies, NpmDefObject obj)
		{
			var bundle = obj.FindBundle();
			if (bundle == null) return false;
			var bundleName = bundle.FindPackageName();
			if (!dependencies.Any(d => d.Name == bundleName))
			{
				dependencies.Add(new Dependency
				{
					Name = bundleName,
					VersionOrPath = bundle.FilePath,
					Guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj))
				});
			}
			return true;
		}

		private static bool MissingDependenciesFoldout
		{
			get => SessionState.GetBool("Needle_ExportInfoSerializedMissingDependencies", true);
			set => SessionState.SetBool("Needle_ExportInfoSerializedMissingDependencies", value);
		}

		private static readonly Dictionary<DependencyProblem, List<Dependency>> MissingDependencies = new Dictionary<DependencyProblem, List<Dependency>>();

		private static void RenderMissingDependencies(ExportInfo exp)
		{
			MissingDependencies.Clear();
			for (var index = 0; index < exp.Dependencies.Count; index++)
			{
				var dep = exp.Dependencies[index];
				var npmdefIsMissing = dep.IsMissingNpmDef();
				var nameIsMissing = string.IsNullOrWhiteSpace(dep.Name);
				if (npmdefIsMissing || nameIsMissing)
				{
					// try resolve from known npmdefs in project
					// this may happen if the npmdef was moved or renamed but the serialized dependency path wasnt updated
					var existing = BundleRegistry.Instance.Bundles.FirstOrDefault(b => b.FindPackageName() == dep.Name);
					if (existing != null)
					{
						dep.VersionOrPath = existing.FilePath;
						if (File.Exists(dep.VersionOrPath))
							dep.Guid = AssetDatabase.AssetPathToGUID(dep.VersionOrPath);
						exp.Dependencies[index] = dep;
					}
					else
					{
						var problemType =  npmdefIsMissing ? DependencyProblem.NpmdefIsMissing : DependencyProblem.NameIsMissing;
						if(!MissingDependencies.ContainsKey(problemType))
							MissingDependencies.Add(problemType, new List<Dependency>());
						MissingDependencies[problemType].Add(dep);
					}
				}
			}
			if (MissingDependencies.Count <= 0) return;

			// using (new ColorScope(Color.yellow))
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Space(3);
				MissingDependenciesFoldout =
					EditorGUILayout.Foldout(MissingDependenciesFoldout, "Missing or invalid dependencies (" + MissingDependencies.Count + ")");
			}
			if (!MissingDependenciesFoldout) return;
			EditorGUILayout.HelpBox(
				"These dependencies are invalid missing npmdef packages that are serialized in this component.",
				MessageType.None);

			foreach (var kvp in MissingDependencies)
			{
				var type = kvp.Key;
				var missing = kvp.Value;
				foreach (var dep in missing)
				{
					using (new EditorGUILayout.HorizontalScope())
					{
						switch (type)
						{
							case DependencyProblem.NpmdefIsMissing:
								EditorGUILayout.LabelField(new GUIContent("Missing \"" + dep.Name + "\"", dep.VersionOrPath));
								break;
							case DependencyProblem.NameIsMissing:
								var info = dep.VersionOrPath;
								var exists = File.Exists(info);
								if (exists) info = Path.GetFileNameWithoutExtension(info);
								EditorGUILayout.LabelField(new GUIContent("Missing name: " + info, dep.VersionOrPath));
								var rect = GUILayoutUtility.GetLastRect();
								if (rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
								{
									var instance = AssetDatabase.LoadAssetAtPath<Object>(dep.VersionOrPath);
									if (instance) EditorGUIUtility.PingObject(instance);
								}
								if (exists && GUILayout.Button("Fix", GUILayout.Width(40)))
								{
									var file = new FileInfo(dep.VersionOrPath);
									var fullPath = file.Directory!.FullName + "/" + Path.GetFileNameWithoutExtension(file.FullName) + "~/package.json";
									if (File.Exists(fullPath))
										EditorUtility.OpenWithDefaultApp(fullPath);
									else
										Debug.LogError("Could not open package.json. Is the package maybe missing? " +
										               fullPath, AssetDatabase.LoadAssetAtPath<Object>(dep.VersionOrPath));
								}
								break;
						}
						if (GUILayout.Button("Remove", GUILayout.Width(60)))
						{
							var value = !string.IsNullOrEmpty(dep.Name) ? dep.Name : dep.VersionOrPath;
							if (EditorUtility.DisplayDialog("Remove dependency",
								    "Do you want to remove the serialized dependency \"" + value + "\" ( " + dep.VersionOrPath + ")?", "Yes", "No"))
							{
								exp.Dependencies.Remove(dep);
								if (!string.IsNullOrEmpty(dep.Name))
								{
									if (exp.Dependencies.Any(d => d.Name != dep.Name) &&
									    PackageUtils.TryReadDependencies(exp.PackageJsonPath, out var dependencies))
									{
										dependencies.Remove(dep.Name);
										PackageUtils.TryWriteDependencies(exp.PackageJsonPath, dependencies);
									}
									Debug.Log("Removed missing dependency to \"" + dep.Name + "\" from " + exp.DirectoryName);
								}
								else
								{
									Debug.LogWarning("You may need to manually edit the package.json at " + exp.PackageJsonPath.AsLink() + " and remove the dependency");
								}
							}
							GUIUtility.ExitGUI();
						}
					}
				}
			}
		}
	}

	internal enum DependencyProblem
	{
		NpmdefIsMissing = 0,
		NameIsMissing = 1
	}
}