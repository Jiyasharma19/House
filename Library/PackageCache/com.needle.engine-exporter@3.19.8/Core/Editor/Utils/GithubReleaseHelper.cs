using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Needle.Engine.Utils
{
	internal static class GithubReleaseHelper
	{
		internal static readonly string changelogPath =
			Path.GetFullPath(Application.dataPath + "/../Temp/Needle/needle-engine-releases.json");

		[InitializeOnLoadMethod]
		internal static async void TryGetLatestReleases()
		{
			while(EditorApplication.isUpdating || EditorApplication.isCompiling) await Task.Delay(1000);
			await Task.Delay(1000);
			
			DownloadIfExportInfoExists();
			void DownloadIfExportInfoExists()
			{
				if (File.Exists(changelogPath)) return;
				if (ExportInfo.Get() == false) return;
				var dir = Path.GetDirectoryName(changelogPath);
				Directory.CreateDirectory(dir!);
				using var client = new WebClient();
				client.Headers.Add("User-Agent", "Needle");
				client.DownloadFile("https://api.github.com/repos/needle-tools/needle-engine-support/releases",
					changelogPath);
			}
		}

	}
}