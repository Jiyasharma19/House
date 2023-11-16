using System.IO;
using UnityEditor;
using UnityEngine;

namespace Needle.Engine.Codegen
{
	internal static class Actions
	{
		[MenuItem("Assets/Generate Typescript Skeleton Component", true)]
		private static bool GenerateTypescript_Validate() => Selection.activeObject is MonoScript;
		[MenuItem("Assets/Generate Typescript Skeleton Component")]
		private static void GenerateTypescript()=> TryGenerateTypescript(Selection.activeObject, out _);
		
		[MenuItem("CONTEXT/Component/Generate Typescript Skeleton Component")]
		[MenuItem("CONTEXT/MonoScript/Generate Typescript Skeleton Component")]
		private static void GenerateTypescript(MenuCommand cmd) => TryGenerateTypescript(cmd.context, out _);

		public static bool TryGenerateTypescript(Object obj, out string filepath)
		{
			filepath = null;
			if (!obj)
				return false;
			var targetDirectory = GetTargetDirectory();
			var codeFilePath = default(string);

			if (obj is MonoScript script)
				codeFilePath = AssetDatabase.GetAssetPath(script);
			else
			{
				var type = obj.GetType();
				var guids = AssetDatabase.FindAssets("t:MonoScript " + type.Name);
				var expectedName = type.Name + ".cs";
				foreach (var guid in guids)
				{
					var path = AssetDatabase.GUIDToAssetPath(guid);
					if (path?.EndsWith(expectedName) == false) continue;
					codeFilePath = path;
					break;
				}
			}

			if (codeFilePath != null)
			{
				var ts = new TypescriptGenerator();
				var result = ts.Run(codeFilePath, targetDirectory);
				filepath = result;
				EditorUtility.OpenWithDefaultApp(result);
			}

			return filepath != null;
		}

		private static string GetTargetDirectory()
		{
			var exportInfo = Object.FindObjectOfType<ExportInfo>();
			if (exportInfo)
			{
				var projectDirectory = Path.GetFullPath(exportInfo.GetProjectDirectory());
				var targetDirectory = projectDirectory + "/src/scripts";
				if (NeedleProjectConfig.TryLoad(projectDirectory, out var config))
				{
					return projectDirectory + "/" + config.scriptsDirectory;
				}
				return targetDirectory;
			}
			
			var tmp = Application.dataPath + "/../Temp/Needle/Codegen/Typescript";
			return tmp;
		}
	}
}