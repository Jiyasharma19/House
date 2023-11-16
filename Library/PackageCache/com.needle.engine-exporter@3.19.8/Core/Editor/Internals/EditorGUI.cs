#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Needle.Engine
{
	public static class PublicEditorGUI
	{
		public delegate Object CustomValidate(Object[] references, Type type, SerializedProperty property);

		private static CustomValidate currentValidate;
		
		public static void ObjectField(Rect position, SerializedProperty property, Type type, GUIContent label, CustomValidate validate = null)
		{
			currentValidate = validate;
			EditorGUI.ObjectField(position, property, type, label, null, OnValidate);
			currentValidate = null;
		}
		
		internal static Object GetPrefabAssetRootGameObject(Object obj)
		{
			return PrefabUtility.GetPrefabAssetRootGameObject(obj);
		}

		private static Object OnValidate(Object[] references, Type type, SerializedProperty property, EditorGUI.ObjectFieldValidatorOptions options)
		{
			if(currentValidate != null)
				return currentValidate(references, type, property);
			return references.FirstOrDefault(r => r && type.IsInstanceOfType(r));
		}
	}
}
#endif