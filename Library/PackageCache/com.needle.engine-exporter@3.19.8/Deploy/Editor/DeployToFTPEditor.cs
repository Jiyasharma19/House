using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Needle.Engine.Core;
using Needle.Engine.Settings;
using Needle.Engine.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Needle.Engine.Deployment
{
	[CustomEditor(typeof(DeployToFTP))]
	public class DeployToFTPEditor : Editor
	{
		private IProjectInfo project;
		private SerializedProperty settings, path, useGzipCompression, overrideGzipCompression;

		private void OnEnable()
		{
			project = ObjectUtils.FindObjectOfType<IProjectInfo>();
			path = serializedObject.FindProperty(nameof(DeployToFTP.Path));
			settings = serializedObject.FindProperty(nameof(DeployToFTP.FTPServer));
			useGzipCompression = serializedObject.FindProperty(nameof(DeployToFTP.UseGzipCompression));
			overrideGzipCompression = serializedObject.FindProperty(nameof(DeployToFTP.OverrideGzipCompression));
		}

		public override void OnInspectorGUI()
		{
			if (project == null)
			{
				EditorGUILayout.HelpBox("No project found - please add a " + nameof(ExportInfo) + " component to you scene", MessageType.Warning);
				return;
			}

			var ftp = target as DeployToFTP;
			if (!ftp)
			{
				base.OnInspectorGUI();
				return;
			}


			ftp.Path = ftp.Path?.TrimStart('.');
			if (string.IsNullOrWhiteSpace(ftp.Path)) ftp.Path = "/";

			var change = new EditorGUI.ChangeCheckScope();

			EditorGUILayout.LabelField("FTP", EditorStyles.boldLabel);

			using (new GUILayout.HorizontalScope())
			{
				EditorGUILayout.PropertyField(settings, new GUIContent("Server"));
				if (!settings.objectReferenceValue)
				{
					if (GUILayout.Button("Create", GUILayout.Width(50)))
					{
						var instance = CreateInstance<FTPServer>();
						AssetDatabase.CreateAsset(instance, "Assets/FTPServer.asset");
						settings.objectReferenceValue = instance;
						serializedObject.ApplyModifiedProperties();
					}
				}
			}

			var server = ftp.FTPServer;
			string key = default;
			if (server)
				server.TryGetKey(out key);
			var password = SecretsHelper.GetSecret(key);

			var hasInvalidDeploymentPath = (server && !server.AllowTopLevelDeployment) &&
			                             (string.IsNullOrWhiteSpace(path.stringValue) ||
			                              path.stringValue.Trim() == "/");

			if (server)
			{
				EditorGUILayout.PropertyField(path, new GUIContent("Path", "The path on the ftp server where you want to deploy your website to."));

				if (hasInvalidDeploymentPath)
				{
					EditorGUILayout.HelpBox("Deployment to the top level directory is not allowed. Please specify a subfolder path.", MessageType.Warning);
				}
				
				using (new EditorGUILayout.HorizontalScope())
				{
					UseGizp.Enabled = EditorGUILayout.Toggle(new GUIContent("Use Gzip", "Enable gzip compression for the files. Make sure your server supports gzip compression."), UseGizp.Enabled);
				}
			}

			if (change.changed)
			{
				serializedObject.ApplyModifiedProperties();
			}

			var hasPassword = !string.IsNullOrWhiteSpace(password);
			var directory = Path.GetFullPath(project.ProjectDirectory) + "/dist";
			var canDeploy = server && hasPassword;


			if (server)
			{
				if (string.IsNullOrWhiteSpace(server.Servername) || string.IsNullOrWhiteSpace(server.Username))
				{
					EditorGUILayout.Space(2);
					EditorGUILayout.HelpBox(
						"Please enter your FTP server, username and password to the FTP server settings. You can get this information from your web provider. Don't worry: your password is not saved with the project and will not be shared.",
						MessageType.Warning);
				}
			}
			else
			{
				EditorGUILayout.Space(2);
				EditorGUILayout.HelpBox("Assign or create a FTP server settings object", MessageType.Info);
			}

			EditorGUILayout.Space(5);

			if (!canDeploy)
			{
				if (!hasPassword && server)
					EditorGUILayout.HelpBox("Server configuration is missing a password", MessageType.None);
			}

			var isDeploying = currentTask != null && !currentTask.IsCompleted;
			using (new EditorGUI.DisabledScope(!canDeploy || hasInvalidDeploymentPath))
			{
				using(new EditorGUI.DisabledScope(isDeploying))
				using (new GUILayout.HorizontalScope())
				{
					var devBuild = NeedleEngineBuildOptions.DevelopmentBuild;
					if ((Event.current.modifiers & EventModifiers.Alt) != 0) devBuild = !devBuild;

					if (GUILayout.Button("Build & Deploy: " + (devBuild ? "Dev" : "Prod"), GUILayout.Height(30)))
					{
						HandleUpload(project, server!.Servername, server.Username, password, server.SFTP, true, devBuild);
					}

					using (new EditorGUI.DisabledScope(!Directory.Exists(directory)))
					{
						if (GUILayout.Button("Deploy Only", GUILayout.Height(30)))
						{
							HandleUpload(project, server!.Servername, server.Username, password,  server.SFTP,false, devBuild);
						}
					}
				}
			}

			var hasRemoteUrl = server && server.RemoteUrlIsValid;
			if (hasRemoteUrl)
			{
				var fullUrl = server.RemoteUrl + "/" + ftp.Path;
				fullUrl = fullUrl.Replace("//", "/").Replace("//", "/");
				// using (new EditorGUI.DisabledScope(!hasRemoteUrl))
				if (GUILayout.Button(
					    new GUIContent("Open in Browser " + Constants.ExternalLinkChar, fullUrl), GUILayout.Height(30)))
				{
					Application.OpenURL(fullUrl);
				}
			}
			
			if (isDeploying)
			{
				EditorGUILayout.HelpBox("Deployment to FTP is in progress...", MessageType.None);	
			}
		}

		private Task<bool> currentTask;
		private CancellationTokenSource cancel;

		private async void HandleUpload(IProjectInfo projectInfo, string server, string username, string password, bool sftp, bool runBuild, bool devBuild)
		{
			var comp = target as DeployToFTP;
			if (!comp) return;

			Debug.Log("Begin uploading...");

			cancel?.Cancel();
			if (currentTask != null && currentTask.IsCompleted == false) await currentTask;
			const int maxUploadDurationInMilliseconds = 10 * 60 * 1000;
			cancel = new CancellationTokenSource(maxUploadDurationInMilliseconds);

			var progId = Progress.Start("FTP Upload", "", Progress.Options.Managed);
			Progress.RegisterCancelCallback(progId, () =>
			{
				if (!cancel.IsCancellationRequested)
				{
					Debug.Log("Cancelling FTP upload...");
					cancel.Cancel();
				}
				return true;
			});

			BuildContext buildContext;
			if (runBuild) buildContext = BuildContext.Distribution(!devBuild);
			else buildContext = BuildContext.PrepareDeploy;

			if (comp.FTPServer.RemoteUrlIsValid)
			{
				var baseUrl = comp.FTPServer.RemoteUrl;
				var remotePath = comp.Path;
				// ensure we don't have double slashes (while keeping https:// intact)
				while(baseUrl.EndsWith("/")) baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
				while (remotePath.StartsWith("/")) remotePath = remotePath.Substring(1);
				while(remotePath.Contains("//")) remotePath = remotePath.Replace("//", "/");
				buildContext.LiveUrl = baseUrl + "/" + remotePath;
			}

			var distDirectory = projectInfo.ProjectDirectory + "/dist";
			var buildResult = false;
			var postBuildMessage = default(string);
			if (runBuild)
			{
				Progress.SetDescription(progId, "Export and Build");
				var dev = NeedleEngineBuildOptions.DevelopmentBuild;
				Debug.Log("<b>Begin building distribution</b>");
				currentTask = Actions.ExportAndBuild(buildContext);
				buildResult = await currentTask;
				postBuildMessage = "<b>Successfully built distribution</b>";
			}
			else
			{
				currentTask = Actions.ExportAndBuild(buildContext);
				buildResult = await currentTask;
			}

			if (cancel.IsCancellationRequested)
			{
				Debug.LogWarning("Upload cancelled");
				return;
			}
			if (!buildResult)
			{
				Debug.LogError("Build failed, aborting FTP upload - see console for errors");
				return;
			}
			if (postBuildMessage != null) Debug.Log(postBuildMessage);

			Debug.Log("<b>Begin uploading</b> " + distDirectory);
			Progress.SetDescription(progId, "Upload " + Path.GetDirectoryName(projectInfo.ProjectDirectory) + " to FTP");
            
			currentTask = Tools.UploadToFTP(server, username, password, distDirectory, comp.Path, sftp, false, cancel.Token);
				
			// currentTask = UploadDirectory(distDirectory, opts);
			var uploadResult = await currentTask;
			if (cancel.Token.IsCancellationRequested)
				Debug.LogWarning("<b>FTP upload was cancelled</b>");
			else if (uploadResult)
			{
				Debug.Log($"<b>FTP upload {"succeeded".AsSuccess()}</b> " + distDirectory);
				if (!string.IsNullOrWhiteSpace(buildContext.LiveUrl))
				{
					Application.OpenURL(buildContext.LiveUrl);
				}
			}
			else Debug.LogError("Uploading failed. Please see console for errors.\n" + distDirectory);
			if (Progress.Exists(progId))
				Progress.Finish(progId);
		}
	}
}