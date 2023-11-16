using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;

namespace Needle.Engine
{
	internal abstract class LicenseCheck
	{
		public static string LicenseEmail
		{
#if UNITY_EDITOR
			get => EditorPrefs.GetString("NEEDLE_ENGINE_license_id", "");
			set => EditorPrefs.SetString("NEEDLE_ENGINE_license_id", value);
#else
			get => "";
			set { }
#endif
		}
		
		public static string LicenseKey
		{
#if UNITY_EDITOR
			get => EditorPrefs.GetString("NEEDLE_ENGINE_license_key", "");
			set => EditorPrefs.SetString("NEEDLE_ENGINE_license_key", value);
#else
			get => "";
			set { }
#endif
		}

		public static event Action<bool> ReceivedLicenseReply;
		
		private static readonly WebClientHelper client = new WebClientHelper("https://urls.needle.tools/license-endpoint");

		public static bool CanMakeLicenseCheck()
		{
			var email = LicenseEmail;
			if (string.IsNullOrWhiteSpace(email))
			{
				return false;
			}
			var key = LicenseKey;
			if (string.IsNullOrWhiteSpace(key))
			{
				return false;
			}
			return true;
		}

		public static async Task<bool> HasValidLicense()
		{
			var email = LicenseEmail;
			if (string.IsNullOrWhiteSpace(email))
			{
				LastLicenseResult = null;
				LastLicenseTypeResult = null;
				return false;
			}
			var key = LicenseKey;
			if (string.IsNullOrWhiteSpace(key))
			{
				LastLicenseResult = null;
				LastLicenseTypeResult = null;
				return false;
			}
			var endpoint = "/?email=" + email + "&key=" + key + "&version=2";
			var res = await client.SendGet(endpoint);
			LastLicenseResult = null;
			LastLicenseTypeResult = null;
			if (res != null)
			{
				NeedleDebug.Log(TracingScenario.NetworkRequests, "License check to " + endpoint + ": " + res.StatusCode);
				var hasLicense = res.IsSuccessStatusCode;
				LastLicenseResult = hasLicense;
				TryParseLicenseType(res);
				ReceivedLicenseReply?.Invoke(hasLicense);
				return LastLicenseResult ?? false;
			}
			return false;
		}

		
#if UNITY_EDITOR
		[InitializeOnLoadMethod]
		private static async void Init()
		{
			// Make sure we have the license check result cached
			if (LastLicenseResult != null) return;
			await HasValidLicense(); 
		}
#endif

		private static async void TryParseLicenseType(HttpResponseMessage msg)
		{
			var body = await msg.Content.ReadAsStringAsync();
			NeedleDebug.Log(TracingScenario.NetworkRequests, body);
			if (body.Trim().StartsWith("{"))
			{
				var json = Newtonsoft.Json.Linq.JObject.Parse(body);
				var license = json["license"]?.ToString();
				LastLicenseTypeResult = license;
				if (license != null)
				{
					switch (license)
					{
						case "enterprise":
							LastLicenseResult = true;
							RequireLicenseAttribute.CurrentLicenseType = LicenseType.Enterprise;
							break;
						case "pro":
							LastLicenseResult = true;
							RequireLicenseAttribute.CurrentLicenseType = LicenseType.Pro;
							break;
						case "indie":
							LastLicenseResult = true;
							RequireLicenseAttribute.CurrentLicenseType = LicenseType.Indie;
							break;
						case "basic":
							LastLicenseResult = false;
							RequireLicenseAttribute.CurrentLicenseType = LicenseType.Basic;
							break;
					}
				}
			}
		}

		internal class LicenseMeta : IBuildConfigProperty
		{
			public string Key => "license";
			public object GetValue(string projectDirectory)
			{
				if (string.IsNullOrWhiteSpace(LastLicenseTypeResult)) return null;
				return LastLicenseTypeResult;
			}
		}

		internal static bool HasLicense => LastLicenseResult == true;
		internal static bool? LastLicenseResult { get; private set; } = null;
		internal static string LastLicenseTypeResult { get; private set; }
		internal static bool LasLicenseResultIsProLicense => LastLicenseTypeResult == "pro";

		// private static DateTime lastLicenseCheckTime = DateTime.MinValue;
		// private static bool lastLicenseCheckResult = false;
		//
		// internal static void ClearLicenseCache()
		// {
		// 	lastLicenseCheckTime = DateTime.MinValue;
		// 	lastLicenseCheckResult = false;
		// }
		//
		// public static async Task<bool> HasValidLicenseCached()
		// {
		// 	if(DateTime.Now - lastLicenseCheckTime < TimeSpan.FromMinutes(10))
		// 		return lastLicenseCheckResult;
		// 	lastLicenseCheckTime = DateTime.Now;
		// 	lastLicenseCheckResult = await HasValidLicense();
		// 	return lastLicenseCheckResult;
		// }
	}
}