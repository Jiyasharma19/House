using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Needle.Engine.Core;
using Needle.Engine.Problems;
using Needle.Engine.Utils;
using Newtonsoft.Json.Linq;
using Semver;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Needle.Engine.Projects
{
	public struct ProjectGenerationOptions
	{
		public bool StartAfterGeneration;
		public IList<Dependency> Dependencies;
	}

	public static class ProjectGenerator
	{
		private static List<ProjectTemplate> _templates;

		public static List<ProjectTemplate> Templates
		{
			get
			{
				if (_templates == null) RefreshTemplates();
				return _templates;
			}
		}

		internal static void MarkTemplatesDirty()
		{
			_templates = null;
		}

		public static void RefreshTemplates()
		{
			_templates ??= new List<ProjectTemplate>();
			_templates.Clear();
			var templateAssets = AssetDatabase.FindAssets("t:" + nameof(ProjectTemplate));
			foreach (var tmp in templateAssets)
			{
				var loaded = AssetDatabase.LoadAssetAtPath<ProjectTemplate>(AssetDatabase.GUIDToAssetPath(tmp));
				if (loaded) _templates.Add(loaded);
			}
			_templates.Sort((a, b) => b.Priority - a.Priority);
		}

		public static Task CreateFromTemplate(string projectDir)
		{
			var path = AssetDatabase.GUIDToAssetPath("456ab5d794b090d4ba4ce834e45a436e");
			return CreateFromTemplate(projectDir, path);
		}

		public static Task CreateFromTemplate(string projectDir,
			ProjectTemplate template,
			ProjectGenerationOptions? options = default)
		{
			if (template.IsRemoteTemplate())
			{
				var exp = ExportInfo.Get();
				return GitActions.CloneProject(template.RemoteUrl, exp.GetProjectDirectory()).ContinueWith(t =>
				{
					var res = t.Result;
					if (res.success)
						return CreateFromTemplate(projectDir, res.localPath, options);
					Debug.LogError("Failed to clone project: see the console for details");
					return Task.FromResult(false);
				}, TaskScheduler.FromCurrentSynchronizationContext());
			}

			return CreateFromTemplate(projectDir, template.GetPath(), options);
		}

		public static async Task CreateFromTemplate(string projectDir,
			string templateDir,
			ProjectGenerationOptions? options = default)
		{
			if (!Directory.Exists(templateDir))
			{
				Debug.LogError("Template not found at " + Path.GetFullPath(templateDir));
				return;
			}
			Analytics.RegisterNewProject(projectDir, new DirectoryInfo(templateDir).Name);
			projectDir = Path.GetFullPath(projectDir);
			if (!Directory.Exists(projectDir)) Directory.CreateDirectory(projectDir);
			SceneView.lastActiveSceneView?.ShowNotification(new GUIContent("Creating web project"));
			CopyTemplate(templateDir, projectDir);
			await PostProcessProjectAndRun(projectDir, options);
		}

		public static event Action<string, ExportInfo> BeforeInstall;

		private static async Task PostProcessProjectAndRun(string projectDir, ProjectGenerationOptions? options)
		{
			var export = ExportInfo.Get();
			if (!export)
			{
				var go = new GameObject("Export");
				go.tag = "EditorOnly";
				export = go.AddComponent<ExportInfo>();
			}
			export.DirectoryName = new Uri(Application.dataPath).MakeRelativeUri(new Uri(projectDir)).ToString();
			export.DirectoryName = export.DirectoryName.Replace("%20", " ");

			BeforeInstall?.Invoke(projectDir, export);
			await Actions.InstallPackage(false, false);
			if (options == null || options.Value.StartAfterGeneration)
			{
				var success = await Builder.Build(false, BuildContext.LocalDevelopment);
				if (success)
				{
					MenuItems.StartDevelopmentServer();
					Application.OpenURL(projectDir);
				}
			}
		}

		private static void CopyTemplate(string sourcePath, string targetPath)
		{
			var sb = new System.Text.StringBuilder();
			sb.AppendLine("Copying template from " + sourcePath);
			try
			{
				var paths = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
				for (var index = 0; index < paths.Length; index++)
				{
					var dirPath = paths[index];
					if (EditorUtility.DisplayCancelableProgressBar("Copy Template", "Create directories",
						    (float)index / paths.Length))
					{
						Debug.Log("Cancelled copying template");
						return;
					}
					Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
				}
				var files = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
				for (var index = 0; index < files.Length; index++)
				{
					var filePath = files[index];
					if (filePath.EndsWith(".meta")) continue;
					// skip template
					if (filePath.EndsWith(".asset") &&
					    AssetDatabase.GetMainAssetTypeAtPath(filePath) == typeof(ProjectTemplate)) continue;
					var target = filePath.Replace(sourcePath, targetPath);
					sb.AppendLine("Copying file: " + target);
					if (EditorUtility.DisplayCancelableProgressBar("Copy Template",
						    "Copy " + filePath + " to " + target, (float)index / files.Length))
					{
						Debug.Log("Cancelled copying template");
						return;
					}
					File.Copy(filePath, target, true);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			finally
			{
				EditorUtility.ClearProgressBar();
				sb.AppendLine("Copying Template: Done");
				Debug.Log(sb);
			}
		}

		public static event Action<string> UpdateVersionsInPackageJson;

		/// <returns>True if a version has changed</returns>
		public static async Task<bool> UpdateVersions(string packageJsonPath)
		{
			var anyHasChanged = false;
			if (packageJsonPath.EndsWith("package.json"))
			{
				var directory = Path.GetDirectoryName(packageJsonPath);
				if (NpmUnityEditorVersions.TryGetVersions(out var versions))
				{
					var t1 = UpdateDependencies(directory, versions, packageJsonPath, "dependencies");
					var t2 = UpdateDependencies(directory, versions, packageJsonPath, "devDependencies");
					var t3 = UpdateDependencies(directory, versions, packageJsonPath, "peerDependencies");
					await Task.WhenAll(t1, t2, t3);
					anyHasChanged |= t1.Result || t2.Result;
				}
			}

			UpdateVersionsInPackageJson?.Invoke(packageJsonPath);
			return anyHasChanged;
		}

		private static async Task<bool> UpdateDependencies(string directory,
			JObject supportedVersions,
			string packageJsonPath,
			string key)
		{
			var changed = false;
			if (PackageUtils.TryReadDependencies(packageJsonPath, out var deps, key))
			{
				var changedVersions = new List<(string key, string value)>();
				foreach (var dep in deps)
				{
					var depName = dep.Key;
					var currentVersion = dep.Value;
					if (supportedVersions.TryGetValue(depName, out var token))
					{
						var version = token.ToString();
						if (currentVersion != version && AllowModification(directory, currentVersion, out var reason))
						{
							if (await NpmUtils.PackageExists(depName, version))
							{
								if (string.IsNullOrWhiteSpace(reason))
									reason = "Recommended version";
								var versionWithoutAlias = version;
								if (PackageUtils.IsAliasVersion(versionWithoutAlias))
									versionWithoutAlias =
										versionWithoutAlias.Substring(
											versionWithoutAlias.LastIndexOf("@", StringComparison.Ordinal));
								var msg =
									$"Updating {depName} from \"{currentVersion}\" to {version}\nIf you want to prevent auto update for a specific version use an alias versioning format like \"npm:{depName}@{versionWithoutAlias}\".\nReason for update: \"{reason}\"";
								Debug.Log(msg);
								changedVersions.Add((depName, version));
							}
							else
							{
								Debug.LogWarning("Skip updating dependency to " + depName + "@" + version +
								                 ": it was not found on npm");
							}
						}
					}
				}
				foreach (var kvp in changedVersions)
				{
					deps[kvp.key] = kvp.value;
					changed = true;
				}

				// Check if needle engine is a local path
				// if yes then we have to install three.js explicitly because otherwise it will not be installed
				if (deps.TryGetValue("@needle-tools/engine", out var engineVersion) &&
				    engineVersion.StartsWith("file:"))
				{
					if (supportedVersions.TryGetValue("three", out var ver))
					{
						if (deps.TryGetValue("three", out var val))
						{
							// Only replace three dependency if it's not a path to a local directory
							if (!PackageUtils.TryGetPath(directory, val, out var path) || !Directory.Exists(path))
								deps["three"] = ver.ToString();
						}
						else deps.Add("three", ver.ToString());
						changed = true;
					}
					else if (!deps.ContainsKey("three"))
					{
						deps.Add("three", "npm:@needle-tools/three@latest");
						changed = true;
					}
				}

				if (changed)
				{
					PackageUtils.TryWriteDependencies(packageJsonPath, deps, key);
				}
			}
			return changed;
		}

		private static bool AllowModification(string directory, string value, out string reason)
		{
			reason = null;

			// This is a special case, the dependency is managed by us
			if (value.StartsWith("npm:@needle-tools/three@"))
			{
				return true;
			}

			// Explicit alias versions should not be updated automatically
			if (PackageUtils.IsAliasVersion(value))
			{
				reason = "Alias version";
				return false;
			}

			if (string.IsNullOrWhiteSpace(value))
			{
				reason = "Empty version";
				return true;
			}

			if (PackageUtils.TryGetPath(directory, value,
				    out var filePath))
			{
				// Dont replace local paths
				if (File.Exists(filePath + "/package.json"))
				{
					reason = "Local path";
					return false;
				}
			}
			else if (value.Contains(".git") || value.StartsWith("git"))
			{
				reason = "Git repository";
				return false;
			}

			return true;
		}
	}
}