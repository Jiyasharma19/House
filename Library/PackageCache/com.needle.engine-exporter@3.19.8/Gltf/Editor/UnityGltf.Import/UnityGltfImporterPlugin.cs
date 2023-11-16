using System.Linq;
using GLTF.Schema;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityGLTF;
using UnityGLTF.Plugins;

namespace Needle.Engine.Gltf.UnityGltf.Import
{
	public class UnityGltfImporterPlugin : GltfImportPlugin
	{
		public override string DisplayName { get; } = "Needle Engine Components Import";

		public override void OnGUI()
		{
			EditorGUILayout.HelpBox("Experimental components import", MessageType.None);
		}

		public override GltfImportPluginContext CreateInstance(GLTFImportContext context)
		{
			return new UnityGltfImport(context);
		}

		internal class UnityGltfImport : GltfImportPluginContext
		{
			private readonly GLTFImportContext importContext;
			private readonly NeedleGltfImportContext needleImportContext;
			private readonly NEEDLE_components_Importer NeedleComponentsImporter = new NEEDLE_components_Importer();
			private readonly NEEDLE_persistent_assets_Importer NeedlePersistentAssetsImporter = new NEEDLE_persistent_assets_Importer();

			public UnityGltfImport(GLTFImportContext importContext)
			{
				this.importContext = importContext;
				needleImportContext = new NeedleGltfImportContext(importContext.FilePath,
					new UnityGltfImporterBridge(importContext.SceneImporter),
					importContext.AssetContext);
			}
			
			public override void OnAfterImportRoot(GLTFRoot root)
			{
				if (root.Extensions != null)
				{
					foreach (var kvp in root.Extensions)
					{
						if (kvp.Value is DefaultExtension defaultExtension)
						{
							var data = defaultExtension.ExtensionData;
							switch (kvp.Key)
							{
								case UnityGltfPersistentAssetExtension.EXTENSION_NAME:
									var ext = data.Children().First() as JObject;
									NeedlePersistentAssetsImporter.OnImport(needleImportContext, ext);
									break;
							}
						}
					}
				}

				NeedleComponentsImporter.OnBeforeImport(needleImportContext);
			}


			public override void OnAfterImportMaterial(GLTFMaterial material,
				int materialIndex,
				Material materialObject)
			{
				needleImportContext.Register("/materials/" + materialIndex, materialObject);
			}

			public override void OnAfterImportNode(Node node, int nodeIndex, GameObject nodeObject)
			{
				needleImportContext.Register("/nodes/" + nodeIndex, nodeObject);

				if (node.Extensions != null)
				{
					foreach (var kvp in node.Extensions)
					{
						if (kvp.Value is DefaultExtension defaultExtension)
						{
							var data = defaultExtension.ExtensionData;
							switch (kvp.Key)
							{
								case UnityGltf_NEEDLE_components_Extension.EXTENSION_NAME:
									var ext = data.Children().First() as JObject;
									NeedleComponentsImporter.OnImport(needleImportContext, nodeObject, nodeIndex, ext);
									break;
							}
						}
					}
				}
			}

			public override async void OnAfterImportScene(GLTFScene scene, int sceneIndex, GameObject sceneObject)
			{
				for (var index = 0; index < importContext.SceneImporter.AnimationCache?.Length; index++)
				{
					var anim = importContext.SceneImporter.AnimationCache[index];
					needleImportContext.Register("/animations/" + index, anim.LoadedAnimationClip);
				}

				NeedlePersistentAssetsImporter.OnAfterImport(needleImportContext);
				NeedleComponentsImporter?.OnAfterImport(needleImportContext);

				await needleImportContext.ExecuteCommands(ImportEvent.AfterImport);

				GltfImporter.RaiseAfterImported(sceneObject);
			}

			public override async void OnAfterImport()
			{
				var ctx = importContext.AssetContext;
				if (needleImportContext != null)
				{
					foreach (var created in needleImportContext.SubAssets)
					{
						if (created)
						{
							ctx.AddObjectToAsset(created.name, created);
							// if (created is AnimatorController controller)
							// {
							// 	foreach (var clip in controller.animationClips)
							// 	{
							// 	}
							// }
						}
					}
					needleImportContext.SubAssets.Clear();

					await needleImportContext.ExecuteCommands(ImportEvent.AfterAssetImport);
				}
			}
		}
	}
}