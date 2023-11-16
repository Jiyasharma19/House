using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Needle.Engine.Utils
{
	internal class NewDeploymentModel
	{
		public string editor = "unity";
		public string editorVersion = Application.unityVersion;
		public string editorProjectName = new DirectoryInfo(Application.dataPath + "/../").Name + "/" +
		                                  SceneManager.GetActiveScene().name;
#if UNITY_EDITOR
		public string userName = CloudProjectSettings.userName;
		public string organization = CloudProjectSettings.organizationName;
#endif
		public string url;
		public string needleEngineVersion;
		public string needleEngineExporterVersion;
		public float size;
		public bool production;

		public NewDeploymentModel(string url, bool devBuild)
		{
			this.url = url;
			this.production = !devBuild;
#if UNITY_EDITOR
			if (userName == "anonymous")
				userName = AnalyticsHelper.GetUserName();
#endif

			var exportInfo = ExportInfo.Get();
			if (exportInfo)
			{
				if (PackageUtils.TryReadDependencies(exportInfo.PackageJsonPath, out var deps))
				{
					if (deps.TryGetValue("@needle-tools/engine", out var version))
					{
						if (PackageUtils.TryGetPath(exportInfo.GetProjectDirectory(), version, out var path))
						{
							var localPackageJson = path + "/package.json";
							if (PackageUtils.TryGetVersion(localPackageJson, out var localVersion))
							{
								version = localVersion + " (local)";
							}
						}
						needleEngineVersion = version;
					}
				}
			}

			var v = ProjectInfo.GetCurrentNeedleExporterPackageVersion(out _);
			this.needleEngineExporterVersion = v;

			if (NeedleProjectConfig.TryGetBuildDirectory(out var dir) && Directory.Exists(dir))
			{
				var allAssets = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
				var totalSize = 0L;
				foreach (var asset in allAssets)
				{
					var info = new FileInfo(asset);
					totalSize += info.Length;
				}
				this.size = totalSize / 1024f / 1024;
			}
		}
	}

	internal static class AnalyticsHelper
	{
		private const string forwardEndpoint = "https://urls.needle.tools/analytics-endpoint-v2";

#if NEEDLE_ENGINE_DEV
		internal static readonly WebClientHelper Api = new WebClientHelper("", "http://localhost:3000");
#else
		internal static readonly WebClientHelper Api = new WebClientHelper(forwardEndpoint);
#endif

		internal static async void SendDeploy(string url, bool devBuild)
		{
			try
			{
				var endpoint = "/api/v2/new/deployment";
				await Api.SendPost(new NewDeploymentModel(url, devBuild), endpoint);
			}
#if NEEDLE_ENGINE_DEV
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
#else
			catch (Exception)
			{
				// ignore
			}
#endif
		}

		private static HttpClient client;
		private static string cachedExternalIp;

		internal static async Task<string> GetExternalIpAddressAsync()
		{
			if (cachedExternalIp != null) return cachedExternalIp;
			try
			{
				client ??= new HttpClient();

				var externalIpString = await client.GetStringAsync("https://api.ipify.org");
				externalIpString = externalIpString
					// .Replace("Current IP Address: ", "")
					.Replace("<br/>", "")
					.Replace("\n", "")
					.Trim();
				;

				cachedExternalIp = externalIpString;
			}
			catch
			{
				cachedExternalIp = "unknown";
			}
			return cachedExternalIp;
		}

		internal static string ExternalIpAddress
		{
			get
			{
				if (cachedExternalIp == null)
					AsyncHelper.RunSync(GetExternalIpAddressAsync);
				return cachedExternalIp;
			}
		}

		internal static string GetIpAddress()
		{
			try
			{
				var strHostName = Dns.GetHostName();
				var ipEntry = Dns.GetHostEntry(strHostName);
				var addr = ipEntry.AddressList;
				return addr[addr.Length - 1].ToString();
			}
			catch (Exception)
			{
				return "";
			}
		}

		internal static string GetUserName()
		{
#if UNITY_EDITOR_WIN
			var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var docs = new DirectoryInfo(folderPath);
			return docs.Parent?.Name ?? Environment.UserName;
#else
			return Environment.UserName;
#endif
		}

#if UNITY_EDITOR
		[InitializeOnLoadMethod]
		private static async void Init()
		{
			await GetExternalIpAddressAsync();
		}
#endif
	}
}