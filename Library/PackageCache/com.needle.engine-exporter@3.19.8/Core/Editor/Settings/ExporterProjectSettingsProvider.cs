using System;
using System.Collections.Generic;
using System.IO;
using Needle.Engine.Utils;
using Semver;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Needle.Engine.Settings
{
	public class ExporterProjectSettingsProvider : SettingsProvider
	{
		public const string SettingsPath = "Project/Needle/Needle Engine";

		[SettingsProvider]
		public static SettingsProvider CreateSettings()
		{
			try
			{
				ExporterProjectSettings.instance.Save();
				return new ExporterProjectSettingsProvider(SettingsPath, SettingsScope.Project);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}

			return null;
		}

		private ExporterProjectSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
			: base(path, scopes, keywords)
		{
		}

		private Vector2 scroll;
		private const int buttonRightWidth = 50;
		private SerializedObject settingsObj;
		private SerializedProperty npmSearchPathsProperty;

		private static bool? hasValidLicense;
		private static DateTime lastLicenseCheckTime;
		private static bool requiresLicenseCheck = false;

		private static async void UpdateLicenseStatus(bool force = false)
		{
			if (!force && DateTime.Now - lastLicenseCheckTime < TimeSpan.FromSeconds(3))
			{
				requiresLicenseCheck = true;
				return;
			}
			if (!LicenseCheck.CanMakeLicenseCheck()) return;
			requiresLicenseCheck = false;
			lastLicenseCheckTime = DateTime.Now;
			hasValidLicense = await LicenseCheck.HasValidLicense();
			InternalEditorUtility.RepaintAllViews();
		}

		private static bool hasCorrectToktxVersionInstalled = true;
		private static DateTime lastToktxCheckTime;
		private static bool CheckIfToktxVersionIsInstalled()
		{
			if (DateTime.Now - lastToktxCheckTime < TimeSpan.FromSeconds(10)) return hasCorrectToktxVersionInstalled;
			lastToktxCheckTime = DateTime.Now;
			return hasCorrectToktxVersionInstalled = Actions.HasMinimumToktxVersionInstalled(out _);
		}

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			var settings = ExporterProjectSettings.instance;
			settings.hideFlags &= ~HideFlags.NotEditable;
			settingsObj = new SerializedObject(settings);
			npmSearchPathsProperty =
				settingsObj?.FindProperty(nameof(ExporterProjectSettings.npmSearchPathDirectories));

			base.OnActivate(searchContext, rootElement);

			async void TestNpmInstalled()
			{
				npmInstalled = await Actions.HasNpmInstalled();
			}

			TestNpmInstalled();

			hasCorrectToktxVersionInstalled = Actions.HasMinimumToktxVersionInstalled(out _);

			hasValidLicense = null;
			UpdateLicenseStatus();
		}

		private bool npmInstalled = true;

		public override bool HasSearchInterest(string searchContext)
		{
			return base.HasSearchInterest(searchContext);
		}

		public override void OnGUI(string searchContext)
		{
			base.OnGUI(searchContext);
			var settings = ExporterProjectSettings.instance;
			if (!settings) return;
			scroll = EditorGUILayout.BeginScrollView(scroll);
			var userSettings = ExporterUserSettings.instance;

			using (var change = new EditorGUI.ChangeCheckScope())
			{
				EditorGUILayout.Space();

				if (!npmInstalled)
				{
					using (new GUILayout.HorizontalScope())
					{
						EditorGUILayout.HelpBox(
							"Please make sure you have nodejs/npm installed. If you did install nodejs recently make sure you restart your computer.",
							MessageType.Warning, true);
						if (GUILayout.Button("Open nodejs website", GUILayout.Height(38)))
						{
							Application.OpenURL("https://nodejs.org/");
						}
					}
				}

				DrawLicenseDetailsUI();

				GUILayout.Space(10);

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
				
				settings.overrideEnterPlaymode =
					EditorGUILayout.Toggle(
						new GUIContent("Override Enter Playmode",
							"When enabled clicking \"Play\" will instead start a local server and export your project to threejs if the current scene is marked for export (has an Unity → threejs exporter component)"),
						settings.overrideEnterPlaymode);
				userSettings.UseVSCode = EditorGUILayout.Toggle(
					new GUIContent("Use VSCode",
						"When enabled all web projects will be opened with vscode by default. If you disable this setting the default code editor will attempted to be used."),
					userSettings.UseVSCode);
				
				GUILayout.Space(5);
				settings.smartExport =
					EditorGUILayout.Toggle(
						new GUIContent("Smart Export",
							"When enabled the exporter will only re-export changed assets to make the export faster → for example if a referenced prefab or scene did not change since the last export it will not be exported again"),
						settings.smartExport);
				settings.useHotReload = EditorGUILayout.Toggle(
					new GUIContent("Use Hot Reload",
						"When enabled typescript changes will applied without reloading the local server (if hot reload fails the browser will refresh normally)"),
					settings.useHotReload);
				
				GUILayout.Space(5);
				settings.allowRunningProjectFixes = EditorGUILayout.Toggle(
					new GUIContent("Allow Project Fixes",
						"When enabled Needle Exporter will attempt to automatically fix issues in your project (like when a .npmDef file was moved so the local package path changed)"),
					settings.allowRunningProjectFixes);
				settings.generateReport =
					EditorGUILayout.Toggle(
						new GUIContent("Generate Report",
							"When enabled exported glb and glTF will write a file that contains source information about referenced assets"),
						settings.generateReport);
				settings.debugMode = EditorGUILayout.Toggle(new GUIContent("Debug Mode", ""), settings.debugMode);

#if UNITY_EDITOR_WIN
				const bool isWindows = true;
#else
				const bool isWindows = false;
#endif
				
#pragma warning disable CS0162 // Unreachable code detected
				// ReSharper disable once ConditionIsAlwaysTrueOrFalse
				if (!isWindows)
				{
					GUILayout.Space(10);
					if (npmSearchPathsProperty != null)
					{
						EditorGUILayout.PropertyField(npmSearchPathsProperty,
							new GUIContent("Additional npm Search Paths",
								"These are only used on OSX and Linux and should be used to declare where your npm installation is or should be searched. For example in \"/usr/local/bin/\""));
					}
				}
#pragma warning restore CS0162 // Unreachable code detected


				if (change.changed)
				{
					settingsObj?.ApplyModifiedProperties();
					settings.Save();
				}

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Links", EditorStyles.boldLabel,
					GUILayout.Width(EditorGUIUtility.labelWidth));
				using (new GUILayout.HorizontalScope())
				{
					if (GUILayout.Button(new GUIContent("Documentation ↗",
						    "Opens the exporter and engine documentation")))
						Application.OpenURL(Constants.DocumentationUrl);
					if (GUILayout.Button(new GUIContent("Samples ↗", "Open Needle Engine Samples website")))
						Application.OpenURL(Constants.SamplesUrl);
					if (GUILayout.Button(new GUIContent("Feedback ↗",
						    "Opens the feedback form to help us improve the workflow (takes ~ 1 minute)")))
						Application.OpenURL(Constants.FeedbackFormUrl);
					if (GUILayout.Button(new GUIContent("License ↗",
						    "Opens the Needle Engine website to buy or manage licenses")))
						Application.OpenURL(Constants.BuyLicenseUrl);
					GUILayout.FlexibleSpace();
				}


				GUILayout.Space(10);
				DrawProjectValidationUI();
			}

			EditorGUILayout.EndScrollView();
		}

		private static void DrawLicenseDetailsUI()
		{
			if (requiresLicenseCheck) UpdateLicenseStatus();

			var buttonWide = GUILayout.Width(buttonRightWidth + 24);
			var currentEmail = LicenseCheck.LicenseEmail;
			var hiddenEmail = currentEmail;
			if (!string.IsNullOrWhiteSpace(hiddenEmail) && hiddenEmail.Length > 5)
			{
				var chars = hiddenEmail.ToCharArray();
				for (var i = 3; i < chars.Length * .6f; i++)
				{
					chars[i] = '*';
				}
				hiddenEmail = new string(chars);
			}

			EditorGUILayout.LabelField(new GUIContent("License", ""), EditorStyles.boldLabel);
			using (new EditorGUILayout.HorizontalScope())
			{
				var newValue = default(string);
				var label = new GUIContent("Email", "The email address used to buy a license");
				if (Event.current.modifiers == EventModifiers.Alt)
					newValue = EditorGUILayout.TextField(label, currentEmail);
				else
					newValue = EditorGUILayout.PasswordField(label, currentEmail);
				if (newValue != currentEmail)
				{
					LicenseCheck.LicenseEmail = newValue.Trim();
					UpdateLicenseStatus();
				}
				if (GUILayout.Button("Manage " + Constants.ExternalLinkChar, buttonWide))
				{
					Application.OpenURL(Constants.ManageLicenseUrl);
				}
			}
			using (new EditorGUILayout.HorizontalScope())
			{
				var currentValue = LicenseCheck.LicenseKey;
				var newValue = default(string);
				var label = new GUIContent("Invoice ID", "One of the active invoice IDs");
				if (Event.current.modifiers == EventModifiers.Alt)
					newValue = EditorGUILayout.TextField(label, currentValue);
				else
					newValue = EditorGUILayout.PasswordField(label, currentValue);
				if (newValue != currentValue)
				{
					LicenseCheck.LicenseKey = newValue.Trim();
					UpdateLicenseStatus();
				}
			}


			if (hasValidLicense != null && !string.IsNullOrWhiteSpace(LicenseCheck.LicenseEmail))
			{
				if (requiresLicenseCheck)
				{
					EditorGUILayout.HelpBox("Waiting for license check... " + hiddenEmail, MessageType.None);
				}
				else if (hasValidLicense == true)
					EditorGUILayout.HelpBox(new GUIContent($"Valid {LicenseCheck.LastLicenseTypeResult.ToUpperInvariant()} license found for {hiddenEmail}", $"Licensed to \"{currentEmail}\"\nYou may need to restart your local web server if it is currently running to see the license being applied"));
				else if (hasValidLicense == false)
				{
					UpdateLicenseStatus();

					if (GUILayout.Button(
						    $"No license found for {hiddenEmail}. Hold ALT to see your entered values in clear text. Please contact hi+license@needle.tools if you think this is an error",
						    EditorStyles.helpBox))
					{
						Debug.Log("hi+license@needle.tools");
						Application.OpenURL(
							"mailto:hi+license@needle.tools&subject=Needle%20Engine%20License%20Question");
					}
				}
			}
			else
			{
				using (new EditorGUILayout.HorizontalScope())
				{
					if (GUILayout.Button("Free for non-commercial use. Get a license at www.needle.tools",
						    EditorStyles.helpBox))
					{
						Application.OpenURL(Constants.BuyLicenseUrl);
					}
					// if(GUILayout.Button("License " + Constants.ExternalLinkChar, buttonWide))
					// {
					// 	Application.OpenURL(Constants.BuyLicenseUrl);
					// }
				}
			}
		}

		private static void ShowDirectoryExistsInfo(string directory, bool requirePackageJson)
		{
			if (string.IsNullOrWhiteSpace(directory)) return;
			var fullLocalRuntime = Path.GetFullPath(directory);
			if (!Directory.Exists(fullLocalRuntime))
			{
				EditorGUILayout.HelpBox("Directory does not exist: " + fullLocalRuntime, MessageType.Warning, true);
			}
			else if (requirePackageJson && !File.Exists(fullLocalRuntime + "/package.json"))
			{
				EditorGUILayout.HelpBox("Directory does not contain package.json: " + fullLocalRuntime,
					MessageType.Warning, true);
			}
		}

		private static void HandleContextMenu(string path, Action<string> update)
		{
			if (Event.current.type == EventType.ContextClick)
			{
				var last = GUILayoutUtility.GetLastRect();
				if (last.Contains(Event.current.mousePosition))
				{
					var m = new GenericMenu();
					if (!string.IsNullOrEmpty(path))
					{
						if (Directory.Exists(path))
							m.AddItem(new GUIContent("Open directory"), false,
								() => Application.OpenURL(Path.GetFullPath(path)));
						if (new Uri(path, UriKind.RelativeOrAbsolute).IsAbsoluteUri)
							m.AddItem(new GUIContent("Make relative"), false,
								() => update(PathUtils.MakeProjectRelative(path)));
						else
							m.AddItem(new GUIContent("Make absolute"), false, () => update(Path.GetFullPath(path)));
					}
					else
					{
#pragma warning disable CS4014
						m.AddItem(new GUIContent("Try fix paths"), false, () => Actions.RunProjectSetup());
#pragma warning restore CS4014
					}
					m.ShowAsContext();
				}
			}
		}

		internal static async void DrawFixSettingsPathsGUI()
		{
			using (new ColorScope(Color.white))
			{
				if (GUILayout.Button("Run Needle project setup", GUILayout.Height(30)))
				{
					if (await Actions.RunProjectSetup( true))
					{
					}
					else
					{
						Debug.LogWarning("Failed to run project setup");
					}
				}
			}
		}



		private static bool _didDownloadToktx
		{
			get => SessionState.GetBool("Needle_DidDownloadToktx", false);
			set => SessionState.SetBool("Needle_DidDownloadToktx", value);
		}

		private static void DrawProjectValidationUI()
		{
			EditorGUILayout.LabelField("Project Validation", EditorStyles.boldLabel, GUILayout.Width(EditorGUIUtility.labelWidth));
			var hasAnyProblem = false;
				
			if (!CheckIfToktxVersionIsInstalled())
			{
				hasAnyProblem = true;
				GUILayout.Space(5);
				using (new GUILayout.HorizontalScope())
				{
					var toktxMessage = "Please install the recommended toktx version to support production builds.\nToktx is used to compress your glb files.";
					if(_didDownloadToktx) toktxMessage += " → Please restart Unity after installing toktx.";
					EditorGUILayout.HelpBox(toktxMessage, MessageType.Warning);
					if (_didDownloadToktx)
					{
						DrawUnityRestartButton();
					}
					else
					{
						if (GUILayout.Button(new GUIContent("Download Toktx Installer",
								    "Toktx is used for compressing your glb output files when making production builds. Click this button to download the recommended toktx version and start the installer!"),
							    GUILayout.Height(38)))
						{
							_didDownloadToktx = true;
							InternalActions.DownloadAndInstallToktx();
						}
					}
				}
			}
            
			if (!hasAnyProblem)
			{
				EditorGUILayout.HelpBox("No problems detected. Everything looks good!", MessageType.Info);
			}

			void DrawUnityRestartButton()
			{
				if (GUILayout.Button(new GUIContent("Restart Unity"), GUILayout.Height(38)))
				{
					EditorApplication.OpenProject(Directory.GetCurrentDirectory());
				}
			}
		}
	}
}