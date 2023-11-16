using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Needle.Engine.Samples
{
	[CreateAssetMenu(menuName = "Needle Engine/Samples/Sample Info")]
	internal class SampleInfo : ScriptableObject
	{
		[UsedImplicitly]
		public string Name
		{
			get => DisplayNameOrName;
			set => DisplayName = value;
		}
		
		[JsonIgnore]
		public string DisplayName;
		public string Description;
		public Texture2D Thumbnail;
		[JsonIgnore]
		public SceneAsset Scene;
		public string LiveUrl;
		public int Priority;
        [JsonIgnore]
		public string DisplayNameOrName => !string.IsNullOrWhiteSpace(DisplayName) ? DisplayName : ObjectNames.NicifyVariableName(name);
		public Tag[] Tags;
		
		[JsonIgnore][HideInInspector]
		public SampleInfo reference;
		
		private void OnValidate()
		{
			if (!Scene)
			{
				var path = AssetDatabase.GetAssetPath(this);
				if (string.IsNullOrWhiteSpace(path)) return;
				var scenes = AssetDatabase.FindAssets("t:SceneAsset", new[] { Path.GetDirectoryName(path) });
				foreach (var guid in scenes)
				{
					var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(guid));
					Scene = scene;
					if (scene)
						break;
				}
			}
		}

#if UNITY_EDITOR
		[OnOpenAsset(100)]
		private static bool OpenAsset(int instanceID, int line)
		{ 
			if (EditorUtility.InstanceIDToObject(instanceID) is SampleInfo sampleInfo)
			{
				if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
					EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sampleInfo.Scene));
				return true;
			}
        
			return false;
		}
#endif

		public override string ToString()
		{
			return DisplayNameOrName + " – " + name;
		}
	}
	    
#if UNITY_EDITOR
	[CustomEditor(typeof(SampleInfo), true)]
	[CanEditMultipleObjects]
	class SampleInfoEditor : Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			// if we have multiple sample assets selected just use the default inspector (for editing multiple tags at once)
			if (targets.Length > 1) return null;
			var t = target as SampleInfo;
			if (!t) return new Label("<null>");

			var isSubAsset = AssetDatabase.IsSubAsset(t);
			var v = new VisualElement();
			foreach (var style in SamplesWindow.StyleSheet)
				v.styleSheets.Add(style);

			if (!EditorGUIUtility.isProSkin) v.AddToClassList("__light");
			v.Add(new SamplesWindow.Sample(t));

			if (!isSubAsset)
			{
				var container = new IMGUIContainer(() => DrawDefaultInspector());
				container.style.marginTop = 20;
				v.Add(container);
			}
			else 
				v.style.maxHeight = 500;
			
			return v;
		}
	}
#endif
}