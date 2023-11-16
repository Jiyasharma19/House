using System.IO;
using Needle.Engine.Core;
using Needle.Engine.Core.References.ReferenceResolvers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_2020_3
using UnityEditor.Experimental.SceneManagement;
#endif

namespace Needle.Engine.Gltf
{
	internal static class EditorActions
	{
		[InitializeOnLoadMethod]
		private static void Init()
		{
			Builder.DoExportCurrentScene = ctx =>
			{
				var scene = SceneManager.GetActiveScene();
				if (string.IsNullOrEmpty(scene.path))
				{
					Debug.LogError("Please save your scene before exporting.");
					return null;
				}
				var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path) as Object;
				GltfReferenceResolver.ClearCache();
				Export.AsGlb(asset, out var path, true);
				return path;
			};
		}
		
		internal static bool TryExportCurrentScene()
		{
			var scene = SceneManager.GetActiveScene();
			var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path) as Object;

			var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			if (prefabStage)
			{
				asset = prefabStage.prefabContentsRoot;
			}

			if (asset)
			{
				GltfReferenceResolver.ClearCache();
				return Export.AsGlb(asset, out _, true);
			}
			return false;
		}
	}
}