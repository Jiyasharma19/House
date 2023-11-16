using System.IO;
using System.Threading.Tasks;
using Needle.Engine.Utils;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Needle.Engine
{
	internal class HiddenProject
	{
		private static readonly string InstallDirectory = Application.dataPath + "/../Temp/@needle-tools-npm-tools";	
		private static readonly string PackageJsonPath = InstallDirectory + "/package.json";
		
		internal static string BuildPipelinePath { get; } =
			$"{InstallDirectory}/node_modules/{Constants.GltfBuildPipelineNpmPackageName}";

		internal static string ComponentCompilerPath { get; } =
			$"{InstallDirectory}/node_modules/{Constants.ComponentCompilerNpmPackageName}";

		internal static string ToolsPath { get; } =
			$"{InstallDirectory}/node_modules/{Constants.ToolsNpmPackageName}";
		
		
		
		
		
		internal static Task<bool> Initialize()
		{
#if UNITY_EDITOR
			if (Directory.Exists(InstallDirectory + "/node_modules"))
			{
				if(didInitialize) return Task.FromResult(true);
			}
			if(initializationTask != null) return initializationTask;
			Debug.Log("Initializing Needle Engine Tools...".LowContrast());
			var t = CreateToolsPackage().ContinueWith(r =>
			{
				didInitialize = true;
				return r.Result;
			}, TaskScheduler.FromCurrentSynchronizationContext());
			initializationTask = t;
			return t;
#else 
			return Task.FromResult(false);
#endif
		}

#if UNITY_EDITOR
		private static bool didInitialize
		{
			get => SessionState.GetBool("NPMToolsDidInitialize", false);
			set => SessionState.SetBool("NPMToolsDidInitialize", value);
		}
		private static Task<bool> initializationTask;
		
		private static Task<bool> CreateToolsPackage()
		{
			Directory.CreateDirectory(InstallDirectory);
			if (!File.Exists(PackageJsonPath))
			{
				File.WriteAllText(PackageJsonPath, "{}");
			}
			var json = File.ReadAllText(PackageJsonPath);
			var obj = JObject.Parse(json);
			obj["name"] = "@needle-tools/editor-tools";
			obj["version"] = "1.0.0";
			obj["description"] = "Npm Tools";
			var deps = new JObject();
			obj["dependencies"] = deps;
			AddDependency(Constants.GltfBuildPipelineNpmPackageName, deps);
			AddDependency(Constants.ToolsNpmPackageName, deps);
			AddDependency(Constants.ComponentCompilerNpmPackageName, deps, "^1.0.0-pre");
			File.WriteAllText(PackageJsonPath, obj.ToString());
			var lockPath = InstallDirectory + "/package-lock.json";
			if (File.Exists(lockPath)) File.Delete(lockPath);
			return ProcessHelper.RunCommand("npm set registry https://registry.npmjs.org && npm install", InstallDirectory, null, true, false, -1);
		}

		private static void AddDependency(string packageName, JObject deps, string defaultVersion = "latest")
		{
			deps.Add(packageName, NpmUnityEditorVersions.TryGetRecommendedVersion(packageName, defaultVersion));
		}


#endif
	}
}