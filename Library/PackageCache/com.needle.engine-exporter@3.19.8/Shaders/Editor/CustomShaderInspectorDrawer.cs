using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Needle.Engine.Shaders
{
	internal static class CustomShaderInspectorDrawer
	{
		[InitializeOnLoadMethod]
		private static void Init()
		{
			InspectorHook.Inject += OnInject;
		}

		private static void OnInject(Editor editor, VisualElement ui)
		{
			if(editor.targets.Length > 1) return;

			var target = editor.target;
			if(target is Material mat) target = mat.shader;
			
			if (target is Shader shader)
			{
				if (shader.name.StartsWith("UnityGLTF/")) return;
				// We can not edit immutable shaders
				var shaderPath = AssetDatabase.GetAssetPath(shader);
				if (shaderPath.Contains("PackageCache")) return;
				// Only shadergraph shaders are supported
				if (!shaderPath.EndsWith(".shadergraph")) return;
				
				var isMarkedForExport = ShaderExporterRegistry.HasExportLabel(shader);
				var customShaderSettings = new VisualElement();
				customShaderSettings.style.paddingLeft = 10;
				customShaderSettings.style.paddingRight = 10;
				customShaderSettings.style.marginTop = 10;
				customShaderSettings.style.marginBottom = 10;
				ui.Add(customShaderSettings);
				customShaderSettings.Add(new Label("Needle Engine — Custom Shader Settings")
				{
					style =
					{
						unityFontStyleAndWeight = FontStyle.Bold,
						marginBottom = 5
					}
				});
				
				var helpBox = new HelpBox();
				var link = new Label("Open Documentation " + Constants.ExternalLinkChar)
				{
					style = { color = new Color(.3f, .6f, 1f), marginTop = 3}
				};
				link.AddManipulator(new Clickable(() =>
				{
					Application.OpenURL(Constants.DocumentationUrlCustomShader);
				}));
				var toggle = new Toggle("Export as Custom Shader");
				toggle.value = isMarkedForExport;
				toggle.RegisterValueChangedCallback(evt =>
				{
					ShaderExporterRegistry.SetExportLabel(shader, evt.newValue);
					OnExportSettingHasChanged();
				});

				
				customShaderSettings.Add(toggle);
				customShaderSettings.Add(helpBox);
				customShaderSettings.Add(link);
				
				void OnExportSettingHasChanged()
				{
					isMarkedForExport = ShaderExporterRegistry.HasExportLabel(shader);
					helpBox.text = isMarkedForExport 
						? "This shader will be export as a custom shader to Needle Engine (Note that we only support exporting custom Unlit shaders at the moment)" 
						: "This shader is not marked as a custom shader for export to Needle Engine. It will instead be exported as a standard glTF material.";
					helpBox.messageType = (HelpBoxMessageType)(isMarkedForExport ? MessageType.Info : MessageType.Warning);
					toggle.value = isMarkedForExport;
				}
				OnExportSettingHasChanged();

				// var imgui = new IMGUIContainer(() =>
				// {
				// 	GUILayout.Space(10);
				// 	GUILayout.Label("Needle Engine — Custom Shader Settings", EditorStyles.boldLabel);
				// 	GUILayout.Space(5);
				// 	if (isMarkedForExport)
				// 		EditorGUILayout.HelpBox("This shader is marked for export", MessageType.Info);
				// 	else
				// 		EditorGUILayout.HelpBox("This shader is not marked for export", MessageType.Warning);
				// 	
				// 	if (GUILayout.Button("Open Shader Editor"))
				// 	{
				// 		
				// 	}
				// });
				// ui.Add(imgui);
			}
		}
	}
}