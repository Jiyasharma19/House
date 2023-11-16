using System;
using System.ComponentModel;
using System.Linq;
using Needle.Engine.Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Needle.Engine.Editors
{
	public class EulaWindow : EditorWindow
	{
		[MenuItem("Help/Needle Engine/EULA Window")]
		public static void Open()
		{
			var window = GetWindow<EulaWindow>();
			if (window) window.Show();
			else
			{
				window = CreateInstance<EulaWindow>();
				window.Show();
			}
		}

		public static bool RequiresEulaAcceptance
		{
			get
			{
				if (CommandlineSettings.EulaAccepted) return false;
				return !ExporterUserSettings.instance.UserReadAndAcceptedEula ||
				       !ExporterUserSettings.instance.UserConfirmedEulaCompliance;
			}
		}


		private void OnEnable()
		{
			minSize = new Vector2(515, 430);
			maxSize = new Vector2(515, 500);
			titleContent = new GUIContent("Needle EULA");

			var root = new VisualElement();
			var header = new VisualElement();
			UIComponents.BuildHeader(header);
			header.style.paddingBottom = 20;
			root.Add(header);
			var mainContainer = new IMGUIContainer();
			mainContainer.onGUIHandler += OnDrawUI;
			root.Add(mainContainer);
			VisualElementRegistry.Register(header);
			rootVisualElement.Add(root);
			rootVisualElement.style.paddingLeft = 10;
			rootVisualElement.style.paddingRight = 10;
			
			acceptedEula = ExporterUserSettings.instance.UserReadAndAcceptedEula;
			acceptedCompliance = ExporterUserSettings.instance.UserConfirmedEulaCompliance;
		}

		private void OnDrawUI()
		{
			DrawGUI();
		}

		private static bool acceptedEula, acceptedCompliance;
		private static Vector2 scroll;
		private static GUIStyle _toggleStyle, _wrappedLabelStyle;

		public static void DrawGUI()
		{
			using var scope = new GUILayout.ScrollViewScope(scroll);
			scroll = scope.scrollPosition;

			_toggleStyle ??= new GUIStyle(EditorStyles.label);
			_toggleStyle.wordWrap = true;
			_toggleStyle.alignment = TextAnchor.MiddleLeft;
			_toggleStyle.clipping = TextClipping.Overflow;

			_wrappedLabelStyle ??= new GUIStyle(EditorStyles.wordWrappedLabel);
			_wrappedLabelStyle.richText = true;

			EditorGUILayout.LabelField(
				"In order to start using Needle Engine you must read and accept the Needle EULA.\nMost notably, the use of Needle Engine Basic is restricted to evaluation purposes or non-commercial work.\n\nThere are several plans for commercial usage, depending on your needs:\n→ <b>Indie</b>: for individuals or companies with total finances below 100.000€ per year\n→ <b>Pro</b>: for companies with total finances above 100.000€ per year\n→ <b>Enterprise</b>: for companies with total finances above 5.000.000€ per year",
				_wrappedLabelStyle);

			GUILayout.Space(10);
			if (GUILayout.Button("Read the full EULA", GUILayout.Height(32))) Application.OpenURL(Constants.EulaUrl);
			GUILayout.Space(20);


			var height = GUILayout.Height(40);

			const string acceptText =
				"I have read and understood the EULA and the restrictions that apply to the use of Needle Engine Basic, Indie and Pro.";
			const string complianceText =
				"I and/or the entity I work for are using Needle Engine for evaluation purposes or non-commercial work and in compliance with the Needle EULA.";
			const string complianceWhenLicensedText =
				"I and/or the entity I work for are using Needle Engine in compliance with the Needle EULA. I have read and understood that Needle Engine licenses are per-seat and I'm the only person using this seat.";
			
			using (new EditorGUI.DisabledScope(!RequiresEulaAcceptance))
			{
				acceptedEula = EditorGUILayout.ToggleLeft(acceptText, acceptedEula, _toggleStyle, height);
				acceptedCompliance = EditorGUILayout.ToggleLeft(
					LicenseCheck.HasLicense ? complianceWhenLicensedText : complianceText, 
					acceptedCompliance, 
					_toggleStyle, 
					height);
			}

			GUILayout.Space(10);
			using (new EditorGUI.DisabledScope(!acceptedEula || !acceptedCompliance || !RequiresEulaAcceptance))
			{
				if (GUILayout.Button("I agree to the EULA", GUILayout.Height(32)))
				{
					ExporterUserSettings.instance.UserReadAndAcceptedEula = acceptedEula;
					ExporterUserSettings.instance.UserConfirmedEulaCompliance = acceptedCompliance;
					ExporterUserSettings.instance.Save();
				}
			}
			
			GUILayout.Space(10);
			
			EditorGUILayout.BeginHorizontal();
			
			if (GUILayout.Button(new GUIContent("See plans ↗",
				    "Opens the Needle Engine website to buy or manage licenses")))
				Application.OpenURL(Constants.BuyLicenseUrl);
			
			if (GUILayout.Button(new GUIContent(LicenseCheck.HasLicense ? "Manage license" : "Activate license")))
				SettingsService.OpenProjectSettings("Project/Needle/Needle Engine");
		
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(10);

			if (!RequiresEulaAcceptance)
			{
				EditorGUILayout.LabelField("Thank you. You can now close this window and start using Needle Engine!", EditorStyles.wordWrappedLabel);
			}
		}
	}
}