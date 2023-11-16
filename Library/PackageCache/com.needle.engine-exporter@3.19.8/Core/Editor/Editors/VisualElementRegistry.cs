using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Needle.Engine.Editors
{
	internal static class VisualElementRegistry
	{
		[InitializeOnLoadMethod]
		private static void Init()
		{
			LicenseCheck.ReceivedLicenseReply += UpdateLicenseElements;
		}
		
		private static readonly List<VisualElement> _registeredElements = new List<VisualElement>();

		internal static async void Register(VisualElement el)
		{
			if (!_registeredElements.Contains(el))
			{
				_registeredElements.Add(el);
				HookEvents(el);
			}
			if (_lastLicenseState == null)
			{
				_lastLicenseState = await LicenseCheck.HasValidLicense();
			}
			UpdateLicenseElements(_lastLicenseState.Value);
		}

		private static bool? _lastLicenseState = false;

		private static void UpdateLicenseElements(bool hasLicense)
		{
			_lastLicenseState = hasLicense;
			foreach (var el in _registeredElements)
			{
				el?.Query<VisualElement>(null, "non-commercial").ForEach(b =>
				{
					if (hasLicense) b.AddToClassList("hidden");
					else b.RemoveFromClassList("hidden");
				});
			}
		}
		
		internal static void HookEvents(VisualElement el)
		{
			el.Query<Button>(null, "link").ForEach(b =>
			{
				b.clicked += () => HandleClick(b);
			});
			el.Query<VisualElement>(null, "link").ForEach(b =>
			{
				if (b is Button) return;
				b.AddManipulator(new Clickable(() => HandleClick(b)));
			});
		}
		
		private static void HandleClick(VisualElement el)
		{
			if (el.name.StartsWith("http"))
			{
				Application.OpenURL(el.name);
			}
			else if (el.name == "preferences:license")
			{
				SettingsService.OpenProjectSettings("Project/Needle/Needle Engine");
			}
			else if (el.name == "preferences:license-window")
			{
				LicenseWindow.ShowLicenseWindow();
			}
		}
	}
}