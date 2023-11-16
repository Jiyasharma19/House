using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Needle.Engine.Components
{
	[Serializable]
	public class WebXRTrackedImage
	{
		[Tooltip("Tracked image marker. Make sure the image has good contrast and unique features to improve the tracking quality.")]
		public ImageReference Image;
		[Tooltip("Make sure this matches your physical marker size! Otherwise the tracked object will \"swim\" above or below the marker.")]
		public float WidthInMeters;
		[Tooltip("The object moved around by the image. Make sure the size matches WidthInMeters.")]
		public AssetReference @Object = null;
		[Tooltip("If true, a new instance of the referenced object will be created for each tracked image. Enable this if you're re-using objects for multiple markers.")]
		public bool CreateObjectInstance = false;
		[Tooltip("Use this for static images (e.g. markers on the floor). Only the first few frames of new poses will be applied to the model. This will result in more stable tracking.")]
		public bool ImageDoesNotMove = false;

		public WebXRTrackedImage(ImageReference image)
		{
			this.Image = image;
		}
	}
	
	public class WebXRImageTracking : MonoBehaviour
	{
		public List<WebXRTrackedImage> TrackedImages = new List<WebXRTrackedImage>();
	}
	
#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(WebXRTrackedImage))]
	internal class WebXRTrackedImageDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var defaultHeight = EditorGUIUtility.singleLineHeight * 5;
			var hasObject = property.FindPropertyRelative(nameof(WebXRTrackedImage.@Object));
			if (!hasObject.FindPropertyRelative(nameof(AssetReference.asset)).objectReferenceValue)
				defaultHeight -= 2 * EditorGUIUtility.singleLineHeight;
			return defaultHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var r = position;
			r.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(r, property.FindPropertyRelative(nameof(WebXRTrackedImage.Image)));
			r.y += EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(r, property.FindPropertyRelative(nameof(WebXRTrackedImage.WidthInMeters)));
			r.y += EditorGUIUtility.singleLineHeight;
			var objectProp = property.FindPropertyRelative(nameof(WebXRTrackedImage.@Object));
			EditorGUI.PropertyField(r, objectProp);
			if (objectProp.FindPropertyRelative(nameof(AssetReference.asset)).objectReferenceValue)
			{
				r.y += EditorGUIUtility.singleLineHeight;
				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(r, property.FindPropertyRelative(nameof(WebXRTrackedImage.CreateObjectInstance)));
				r.y += EditorGUIUtility.singleLineHeight;
				EditorGUI.PropertyField(r, property.FindPropertyRelative(nameof(WebXRTrackedImage.ImageDoesNotMove)));
				EditorGUI.indentLevel--;
			}
		}
	}
	
#endif
}