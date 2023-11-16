﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Needle.Engine.Core;
using Needle.Engine.Interfaces;
using Needle.Engine.Settings;
using Needle.Engine.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Needle.Engine.ProjectBundle
{
	[UsedImplicitly]
	public class BundleInstallDependencies : IBuildStageCallbacks
	{
		public Task<bool> OnBuild(BuildStage stage, ExportContext context)
		{
			switch (stage)
			{
				case BuildStage.PostBuildScene:
					return EnsureLocalDependenciesAreInstalled(context.ProjectDirectory, new List<string>());
			}
			return Task.FromResult(true);
		}

		// this minimal model is only used for reading the current content right now
		[Serializable]
		private class PackageJsonModel
		{
			[JsonProperty("devDependencies"), UsedImplicitly] 
			public Dictionary<string, string> devDependencies;
		}

		/// <summary>
		/// finds all local paths in referenced dependencies and attempts to resolve those to the correct paths in your project for building
		/// </summary>
		private static async Task<bool> EnsureLocalDependenciesAreInstalled(string currentDirectory, ICollection<string> resolved)
		{
			// preventing infinite recursion if someone built a cyclic reference
			if (resolved.Contains(currentDirectory)) return true;
			resolved.Add(currentDirectory);
			
			var failed = false;
			// TODO: if we have a npmdef that references another npmdef we need to handle devDependencies as well here
			var packageJsonPath = currentDirectory + "/package.json";
			if (PackageUtils.TryReadDependencies(packageJsonPath, out var deps))
			{
				foreach (var dep in deps)
				{
					if (PackageUtils.IsPath(dep.Value))
					{
						if (PackageUtils.TryGetPath(currentDirectory, dep.Value, out var fullPath))
						{
							if (!Directory.Exists(fullPath))
							{
								Debug.LogWarning("Can not run install for " + dep.Key + " because dependency not found at " + fullPath + "\nPlease check your dependencies in " + packageJsonPath);
								continue;
							}

							// ensure that the dependencies are installed
							if (!await TryResolveDependencies(fullPath, resolved))
							{
								failed = true;
								Debug.LogWarning("Dependency not found: \"" + dep.Key + "\" : \"" + dep.Value + "\"\nFullPath: " + fullPath);
								continue;
							}
							
							var isInstalled = Directory.Exists(fullPath + "/node_modules");
							if (isInstalled) continue;

							if (BundleRegistry.HasNpmDef(dep.Key))
							{
								Debug.Log("<b>Install dependency</b>: " + dep.Key + " at " + fullPath.AsLink());
								if (!await ProcessHelper.RunCommand($"npm install", fullPath, null, true, true, -1))
								{
									failed = true;
									Debug.LogWarning("Installing dependency " + dep.Key + " did fail\n" + fullPath);
								}
							}
						}
					}
				}
			}
			return failed;
		}

		private static async Task<bool> TryResolveDependencies(string directory, ICollection<string> resolvedDirectories)
		{
			var packageJsonPath = directory + "/package.json";
			if (!File.Exists(packageJsonPath)) return false;
			var json = File.ReadAllText(packageJsonPath);
			var package = JsonConvert.DeserializeObject<PackageJsonModel>(json);
			if (package != null && package.devDependencies != null)
			{
				var newPaths = new List<(string key, string value)>();
				// we currently only fix/update dev dependencies
				foreach (var dep in package.devDependencies)
				{
					// check if its a path
					if (PackageUtils.TryGetPath(directory, dep.Value, out var fullPath))
					{
						if (!Directory.Exists(fullPath))
						{
							switch (dep.Key)
							{
								// resolve referenced npmdefs
								default:
									var bundle = BundleRegistry.Instance.Bundles.FirstOrDefault(b => b.FindPackageName() == dep.Key);
									if (bundle != null)
									{
										var bundleDir = bundle.PackageDirectory;
										if (Directory.Exists(bundleDir))
										{
											var path = PackageUtils.GetFilePath(directory, bundleDir);
											newPaths.Add((dep.Key, path));
											// recursive install and update references in that bundle
											await EnsureLocalDependenciesAreInstalled(bundleDir, resolvedDirectories);
										}
										else Debug.LogWarning("Failed to find bundle package directory: " + dep.Key + " in " + packageJsonPath);
									}
									else
									{
										if(NpmUnityEditorVersions.TryGetRecommendedVersion(dep.Key, out var version))
										{
											newPaths.Add((dep.Key, version));
										}
										else if (dep.Key == "three")
										{
											newPaths.Add((dep.Key, "*"));
										}
										else
										{
											Debug.LogWarning("Failed to resolve filepath dependency: " + dep.Key);
										}
									}
									break;
							}
						}
					}
				}

				var opts = new PackageUtils.Options() { MakePathRelativeToPackageJson = true };
				foreach (var kvp in newPaths)
				{
					PackageUtils.ReplaceDevDependency(packageJsonPath, kvp.key, kvp.value, opts);
				}
				
				return true;
			}
			return false;
		}
	}
}