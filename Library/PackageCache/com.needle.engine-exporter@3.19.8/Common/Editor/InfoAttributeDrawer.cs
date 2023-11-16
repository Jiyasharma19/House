﻿using System;
using UnityEditor;
using UnityEngine;

namespace Needle.Engine
{
	[CustomPropertyDrawer(typeof(InfoAttribute))]
	public class InfoAttributeDrawer : DecoratorDrawer
	{
		private static GUIStyle _richInfoStyle, _fallback;
		private static GUIStyle richInfoStyle {
			get
			{
				if (_richInfoStyle != null) return _richInfoStyle;
				try
				{
					_richInfoStyle = new GUIStyle(EditorStyles.helpBox);
					_richInfoStyle.richText = true;
					return _richInfoStyle;
				}
				catch (Exception)
				{
					// ignored
					return _fallback ??= new GUIStyle();
				}
			}
		}

		public override float GetHeight()
		{
			var info = (attribute as InfoAttribute);
			if (info == null || string.IsNullOrEmpty(info.message)) return 0;
			var content = new GUIContent(info.message);
			var height = richInfoStyle.CalcHeight(content, Screen.width);
			height += 4;
			return height;
		}

		public override void OnGUI(Rect position)
		{
			if(position.height > 0 && attribute is InfoAttribute info && !string.IsNullOrEmpty(info.message)) 
				DrawHelpBox(ref position, info);
		}

		private static void DrawHelpBox(ref Rect position, InfoAttribute info)
		{
			position.y += 1;
			position.height -= 4;
			GUI.Label(position, EditorGUIUtility.TrTextContentWithIcon(info.message, (MessageType)info.type), richInfoStyle);
		}
	}
}