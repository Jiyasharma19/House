﻿using System;
using System.IO;
using System.Linq;
using Needle.Engine.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Needle.Engine.Settings
{
	[FilePath("ProjectSettings/NeedleExporterSettings.asset", FilePathAttribute.Location.ProjectFolder)]
	public class ExporterProjectSettings : ScriptableSingleton<ExporterProjectSettings>
	{
		public bool overrideEnterPlaymode = true;
		// public bool overrideBuildSettings = true;
		public bool smartExport = false;
		public bool debugMode = false;
		public bool generateReport = false;
		public bool allowRunningProjectFixes = true;
		public string[] npmSearchPathDirectories = Array.Empty<string>();
		public bool useHotReload = true;

		internal void Save()
		{
			Undo.RegisterCompleteObjectUndo(this, "Save Needle Exporter Settings");
			base.Save(true);
		}

		[InitializeOnLoadMethod]
		private static void Init()
		{
			ProcessHelper.GetAdditionalNpmSearchPaths += separator =>
			{
				if (instance.npmSearchPathDirectories.Length <= 0)
				{
					var path = NpmUtils.TryFindNvmInstallDirectory();
					if (path != null)
					{
						Debug.Log("Add npm search path: " + path);
						instance.npmSearchPathDirectories = new[] { path };
					}
				}
				return string.Join(separator, instance.npmSearchPathDirectories.Where(e => !string.IsNullOrWhiteSpace(e)));
			};
		}
	}
}