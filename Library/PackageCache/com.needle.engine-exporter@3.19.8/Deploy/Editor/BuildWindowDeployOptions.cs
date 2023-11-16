using System;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using Needle.Engine.Core;
using Needle.Engine.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Needle.Engine.Deployment
{
	public class BuildWindowDeployOptions : INeedleBuildPlatformGUIProvider
	{
		private static Type[] deploymentComponents;

		public void OnGUI(NeedleEngineBuildOptions options)
		{
			using (new EditorGUI.DisabledScope(Actions.IsRunningBuildTask))
			{
			}
			using (new EditorGUILayout.VerticalScope())
			{
				var main = EditorStyles.wordWrappedLabel;

				deploymentComponents = TypeCache.GetTypesWithAttribute<DeploymentComponentAttribute>()
					.Where(t => typeof(MonoBehaviour).IsAssignableFrom(t)).ToArray();

				GUILayout.Label("Add Deployment components", EditorStyles.boldLabel);
				GUILayout.Space(6);
				EditorGUILayout.BeginHorizontal();
				var i = 0;
				foreach (var type in deploymentComponents)
				{
					if (i++ >= 3)
					{
						i = 0;
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.BeginHorizontal();
					}
					if (GUILayout.Button(ObjectNames.NicifyVariableName(type.Name)))
					{
						var existing = Object.FindObjectOfType(type);
						if (existing)
						{
							EditorGUIUtility.PingObject(existing);
							Selection.activeObject = existing;
						}
						else
						{
							var exp = ExportInfo.Get();
							if (exp)
							{
								var gameObject = exp.gameObject;
								Debug.Log("Add " + type.Name + " component to " + gameObject, gameObject);
								Undo.AddComponent(exp.gameObject, type);
								EditorGUIUtility.PingObject(exp);
							}
						}
					}
					
				}
				EditorGUILayout.EndHorizontal();
				
				GUILayout.Space(5);
				using (new EditorGUILayout.HorizontalScope())
				{
					EditorGUILayout.LabelField(
						new GUIContent(
							"Learn more about available deployment options by visiting the Needle Engine documentation."),
						main);
					if (GUILayout.Button("Open Documentation"))
						Help.BrowseURL(Constants.DocumentationUrlDeployment);
				}

			}
		}
	}
}