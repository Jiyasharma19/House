using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GLTF.Schema;
using Needle.Engine.Core;
using Needle.Engine.Core.References;
using Needle.Engine.Utils;
using Needle.Engine.Writer;
using Newtonsoft.Json.Linq;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityGLTF;

namespace Needle.Engine.Gltf.UnityGltf
{
	/// <summary>
	/// Base extension export component for UnityGltf, called from GltfObject component to invoke events on ExtensionHandler implementors
	/// </summary>
	[GltfExportHandler(GltfExporterType.UnityGLTF)]
	public class UnityGltfExportHandler : IGltfExportHandler
	{
		private List<IGltfExtensionHandler> extensionHandlers;
		private GltfExportContext currentContext;
		private UnityGltfBridge bridge;
		private Transform transform;
		private GLTFSettings settings;

		public GltfExporterType Type => GltfExporterType.UnityGLTF;

		internal GltfExportContext Context => currentContext;
		internal List<IGltfExtensionHandler> ExtensionHandlers => extensionHandlers;

		internal static void EnsureExportSettings(GLTFSettings settings)
		{
			settings.ExportNames = true;
			settings.ExportFullPath = false;
			settings.RequireExtensions = false;

			settings.UseMainCameraVisibility = false;
			settings.ExportDisabledGameObjects = true;

			settings.TryExportTexturesFromDisk = false;
			settings.UseTextureFileTypeHeuristic = true;
			settings.DefaultJpegQuality = 100;

			settings.ExportAnimations = true;
			settings.BakeAnimationSpeed = false;
			settings.UseAnimationPointer = true;
			settings.UniqueAnimationNames = false;
			settings.BakeSkinnedMeshes = false;

			settings.BlendShapeExportProperties = GLTFSettings.BlendShapeExportPropertyFlags.PositionOnly | GLTFSettings.BlendShapeExportPropertyFlags.Normal;
			settings.BlendShapeExportSparseAccessors = true;
			settings.ExportVertexColors = true;
			
			settings.UseCaching = true;
		}

		public UnityGltfExportHandler()
		{
			Builder.BuildStarting += alreadyExported.Clear;
		}

		private static readonly List<string> alreadyExported = new List<string>();
		private static readonly Stack<string> exportStack = new Stack<string>();

		public Task<bool> OnExport(Transform t, string path, IExportContext ctx)
		{
#if UNITY_EDITOR
			if(ctx.ParentContext == null) alreadyExported.Clear();
			if (alreadyExported.Contains(path))
			{
				Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "{0}",$"glTF {Path.GetFileName(path)} already exported in this run → will not export again".LowContrast());
				return Task.FromResult(true);
			}
			alreadyExported.Add(path);
			
			this.transform = t;
			extensionHandlers ??= InstanceCreatorUtil.CreateCollectionSortedByPriority<IGltfExtensionHandler>();


			if (!settings) settings = ScriptableObject.CreateInstance<GLTFSettings>();
			EnsureExportSettings(settings);

			var exporter = ExportUtils.GetExporter(t, out var exportOptions, settings);
			exportOptions.BeforeSceneExport += OnBeforeExport;
			exportOptions.AfterNodeExport += OnAfterNodeExport;
			exportOptions.AfterSceneExport += OnAfterExport;
			exportOptions.BeforeMaterialExport += OnBeforeMaterialExport;
			exportOptions.AfterMaterialExport += OnAfterMaterialExport;
			exportOptions.BeforeTextureExport += OnBeforeTextureExport;
			exportOptions.AfterTextureExport += OnAfterTextureExport;
			exportOptions.AfterPrimitiveExport += OnAfterPrimitiveExport;

			bridge = new UnityGltfBridge(exporter);
			this.currentContext = new GltfExportContext(this, path, t, ctx, ctx.TypeRegistry, bridge, GltfValueResolver.Default, exporter);
			this.currentContext.AssetsDirectory = Path.GetDirectoryName(path);
			this.currentContext.AssetExtension = new UnityGltfPersistentAssetExtension(this.currentContext);
			this.currentContext.DependencyRegistry = new DependencyRegistry(this.currentContext);

			var animationPreviewState = AnimationWindowUtil.IsPreviewing();
			if (animationPreviewState) AnimationWindowUtil.StopPreview();
			try
			{
				using (new CultureScope())
				{
					var tabString = string.Join("", exportStack.Select(s => "-"));
					if (tabString.Length > 0) tabString = "↳" + tabString;
					Debug.Log($"{tabString}\u2192 <b>Export</b> {t.name}\n{path}");
					var progressTitle = DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss") + " - Exporting glb";
					var progressInfo = "Export " + Path.GetFileName(path);
					// TODO: we need better information here (which component did trigger the export, what is the original source asset or scene object)
					exportStack.Push(path);
					EditorUtility.DisplayProgressBar(progressTitle, progressInfo, 0f);
					ExportUtils.ExportWithUnityGltf(exporter, path, path.EndsWith(".glb"));
					EditorUtility.DisplayProgressBar(progressTitle, progressInfo, .5f);
					OnExportFinished();
					EditorUtility.DisplayProgressBar(progressTitle, progressInfo + " done", 1f);
				}
			}
			catch (IOException io)
			{
				var stackInfo = exportStack.Reverse().Select(s => s.GetShortDisplayPath());
				Debug.LogError(
					$"<b>Failed to export</b> ↑ {Path.GetFileName(path)} - you seem to have objects with the same name referencing each other.\nExport Stack: {string.Join(" → ", stackInfo)}\n\nException thrown: {io}");
				if(SceneView.lastActiveSceneView)
					SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Error during export: see console for details"));
				// TODO: would be good to automatically handle those cases and change the output path so exports would STILL work and it's not a users problem but we currently don't have control here anymore over changing the output path
				// if (!path.Contains("-1."))
				// {
				// 	var dir = Path.GetDirectoryName(path);
				// 	var name = Path.GetFileNameWithoutExtension(path);
				// 	var ext = Path.GetExtension(path);
				// 	var newPath = dir + "/" + name + "-1" + ext;
				// 	return OnExport(t, ref newPath, ctx);
				// }
				return Task.FromResult(false);
			}
			finally
			{
				exportStack.Pop();
				if (animationPreviewState) AnimationWindowUtil.StartPreview();
				this.OnCleanup();
				EditorUtility.ClearProgressBar();
			}
#endif
			return Task.FromResult(true);
		}

		private void OnBeforeExport(GLTFSceneExporter exporter, GLTFRoot root)
		{
			TextureExportHandlerRegistry.BeforeExport();

			if (root.Asset != null)
			{
				root.Asset.Generator = "Needle Engine";
			}
			
			foreach (var handler in extensionHandlers)
			{
				handler.OnBeforeExport(currentContext);
			}
		}

		private void OnAfterNodeExport(GLTFSceneExporter exporter, GLTFRoot root, Transform t, Node node)
		{
			foreach (var h in extensionHandlers)
			{
				h.OnAfterNodeExport(currentContext, t, exporter.GetTransformIndex(t));
			}
		}

		private void OnBeforeTextureExport(GLTFSceneExporter exporter, ref GLTFSceneExporter.UniqueTexture obj, string textureSlot)
		{
			var textureSettings = obj.FromUnique();
			foreach (var handler in extensionHandlers)
			{
				handler.OnBeforeTextureExport(currentContext, ref textureSettings, textureSlot);
			}
			textureSettings.ApplyToUnique(ref obj);
		}

		private void OnAfterTextureExport(GLTFSceneExporter exporter, GLTFSceneExporter.UniqueTexture obj, int id, GLTFTexture tex)
		{
			var textureSettings = obj.FromUnique();
			foreach (var handler in extensionHandlers)
			{
				handler.OnAfterTextureExport(currentContext, id, textureSettings);
			}
		}

		private bool OnBeforeMaterialExport(GLTFSceneExporter exporter, GLTFRoot root, Material material, GLTFMaterial node)
		{
			var id = root.Materials.IndexOf(node);
			foreach (var handler in extensionHandlers)
			{
				handler.OnBeforeMaterialExport(currentContext, material, id);
			}
			return false;
		}

		private void OnAfterMaterialExport(GLTFSceneExporter exporter, GLTFRoot root, Material material, GLTFMaterial node)
		{
			var id = root.Materials.IndexOf(node);
			foreach (var handler in extensionHandlers)
			{
				handler.OnAfterMaterialExport(currentContext, material, id);
			}
			currentContext.Debug?.WriteDebugReferenceInfo(node, "material", material);
		}

		private readonly List<object> primitiveExtensions = new List<object>();

		private void OnAfterPrimitiveExport(GLTFSceneExporter exporter, Mesh mesh, MeshPrimitive primitive, int index)
		{
			if (index > 0) return;
			primitiveExtensions.Clear();
			foreach (var handler in extensionHandlers)
			{
				handler.OnAfterMeshExport(currentContext, mesh, primitiveExtensions);
			}

			if (primitiveExtensions.Count > 0)
			{
				var root = exporter.GetRoot();
				root.ExtensionsUsed ??= new List<string>();
			
				foreach (var ext in primitiveExtensions)
				{
					var name = ext.GetType().Name;
					var obj = JObject.Parse(this.Context.Serializer.Serialize(ext));
					if (primitive.Extensions != null && primitive.Extensions.ContainsKey(name))
						primitive.Extensions[name] = new UnityGltfOpaqueExtension(name, obj);
					else
						primitive.AddExtension(name, new UnityGltfOpaqueExtension(name, obj));

					if(!root.ExtensionsUsed.Contains(name))
						root.ExtensionsUsed.Add(name);
				}
			}
		}

		private void OnAfterExport(GLTFSceneExporter exporter, GLTFRoot root)
		{
			foreach (var handler in extensionHandlers)
			{
				Profiler.BeginSample("OnAfterExport: " + handler.GetType());
				handler.OnAfterExport(currentContext);
				Profiler.EndSample();
			}

			if (this.currentContext.DependencyRegistry?.Count > 0)
			{
				root.AddExtension(UnityGltf_NEEDLE_gltf_dependencies.EXTENSION_NAME,
					new UnityGltf_NEEDLE_gltf_dependencies(this.currentContext, this.currentContext.DependencyRegistry));
				root.ExtensionsUsed ??= new List<string>();
				if(!root.ExtensionsUsed.Contains(UnityGltf_NEEDLE_gltf_dependencies.EXTENSION_NAME))
					root.ExtensionsUsed.Add(UnityGltf_NEEDLE_gltf_dependencies.EXTENSION_NAME);
			}

			currentContext.AssetExtension.AddExtension(currentContext.Bridge);
		}

		private void OnExportFinished()
		{
			foreach (var handler in extensionHandlers)
			{
				handler.OnExportFinished(currentContext);
			}
		}

		private void OnCleanup()
		{
			foreach (var handler in extensionHandlers)
			{
				handler.OnCleanup();
			}

			this.currentContext?.Debug?.Flush();
		}
	}
}