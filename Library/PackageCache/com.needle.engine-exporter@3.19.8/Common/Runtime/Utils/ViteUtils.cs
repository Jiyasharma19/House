﻿using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace Needle.Engine.Utils
{
	public static class ViteUtils
	{
		public static async Task<bool> Preview(string projectDirectory)
		{
			var packageJsonPath = projectDirectory + "/package.json";
			if (PackageUtils.TryGetScripts(packageJsonPath, out var scripts))
			{
				var addedPreviewScript = false;
				if (!scripts.ContainsKey("preview"))
				{
					addedPreviewScript = true;
					scripts.Add("preview", "vite preview");
					if (!PackageUtils.TryWriteScripts(packageJsonPath, scripts))
					{
						Debug.LogWarning("Missing preview script in package.json. Add \"preview\" : \"vite preview\" to your package.json at " + packageJsonPath.AsLink());
						return false;
					}
					Debug.Log("Added temporary preview script: \"preview\" : \"vite preview\" because no preview script was found in package.json: " + packageJsonPath);
				}
				// https://vitejs.dev/guide/cli.html#vite-preview
				var preview = ProcessHelper.RunCommand("npm run preview -- --host --port 3300 --open true", projectDirectory, null, true, true);
				if (addedPreviewScript)
				{
					await Task.Delay(1000);
					scripts.Remove("preview");
					PackageUtils.TryWriteScripts(packageJsonPath, scripts);
				}
				return await preview;
			}
			return false;
		}
		
		public const string ViteConfigName = "vite.config.js";

		/// <param name="projectDirectory"></param>
		/// <param name="enable">Null to not change it</param>
		/// <param name="compressionWasEnabled"></param>
		/// <returns></returns>
		public static bool ChangeGzipCompression(string projectDirectory, bool? enable, out bool compressionWasEnabled)
		{
			compressionWasEnabled = false;
			var foundViteCompression = false;
			var path = projectDirectory + "/" + ViteConfigName;
			if (File.Exists(path))
			{
				var lines = File.ReadAllLines(path);
				var changed = false;
				for (var index = 0; index < lines.Length; index++)
				{
					var line = lines[index];
					var viteCompressionIndex = line.IndexOf("viteCompression(", StringComparison.OrdinalIgnoreCase);
					if (viteCompressionIndex >= 0)
					{
						var isUsingNeedleGZipPlugin = line.IndexOf("needle", StringComparison.OrdinalIgnoreCase) >= 0;
						if (isUsingNeedleGZipPlugin)
						{
							return false;
						}
						foundViteCompression = true;
						var commentIndex = line.IndexOf("//", StringComparison.OrdinalIgnoreCase);
						var isCommentedOut = commentIndex >= 0 && commentIndex < viteCompressionIndex;
						compressionWasEnabled = !isCommentedOut;
						if (enable.HasValue)
						{
							if (!enable.Value && !isCommentedOut)
							{
								changed = true;
								lines[index] = line.Insert(viteCompressionIndex, "//");
							}
							else if (enable.Value && isCommentedOut)
							{
								changed = true;
								lines[index] = line.Remove(commentIndex, 2);
							}
						}
					}
				}
				if (changed)
				{
					File.WriteAllLines(path, lines);
				}
			}
			return foundViteCompression;
		}

		private static readonly Regex PortRegex = new Regex("port: ?(?<port>\\d{3,4})", RegexOptions.Compiled);
		public static bool TryReadPort(string projectDirectory, out int port)
		{
			var path = projectDirectory + "/" + ViteConfigName;
			if (File.Exists(path))
			{
				var text = File.ReadAllText(path);
				var match = PortRegex.Match(text);
				if (match.Success)
				{
					return int.TryParse(match.Groups["port"].Value, out port);
				}
			}

			port = -1;
			return false;
		}
	}
}