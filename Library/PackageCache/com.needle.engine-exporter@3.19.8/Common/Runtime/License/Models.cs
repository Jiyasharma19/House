using System;
using Needle.Engine.Utils;
using UnityEditor;
using UnityEngine;

namespace Needle.Engine
{
	internal class BaseModel
	{
		public string editor;
		public string editorVersion;
		public bool isPro;
		public string userName;
		public string organization;
		public string ipAddress;
		public string externalIpAddress;
		public string licenseEmail;
		public string licenseKey;

		public BaseModel()
		{
			editor = "unity";
			editorVersion = Application.unityVersion;
			isPro = Application.HasProLicense();
#if UNITY_EDITOR
			userName = CloudProjectSettings.userName;
			organization = CloudProjectSettings.organizationId;
#endif
			if (userName == "anonymous" || string.IsNullOrWhiteSpace(userName))
				userName = AnalyticsHelper.GetUserName();
			ipAddress = AnalyticsHelper.GetIpAddress();
			externalIpAddress = AnalyticsHelper.ExternalIpAddress;
			licenseEmail = LicenseCheck.LicenseEmail;
			licenseKey = LicenseCheck.LicenseKey;
		}
	}

	internal class NewInstallationModel : BaseModel
	{
		public string os;
		public string osDeviceName;
		public string osUserName;
		public string osDomainName;
		public string deviceId;
		public string graphicsDevice;
		public string systemLanguage;
		public string exporterVersion;

		public NewInstallationModel()
		{
			os = SystemInfo.operatingSystem;
			osDeviceName = SystemInfo.deviceName;
			osUserName = AnalyticsHelper.GetUserName();
			osDomainName = Environment.UserDomainName;
			deviceId = SystemInfo.deviceUniqueIdentifier;
			graphicsDevice = SystemInfo.graphicsDeviceName;
			systemLanguage = Application.systemLanguage.ToString();
			exporterVersion = ProjectInfo.GetCurrentNeedleExporterPackageVersion(out _);
		}
	}

	internal class UserCreatedProjectFromTemplateModel : BaseModel
	{
		public string projectName;
		public string templateName;

		public UserCreatedProjectFromTemplateModel(string projectName, string templateName)
		{
			this.projectName = projectName;
			this.templateName = templateName;
		}

		internal static string AnonymizeProjectName(string name)
		{
			var unityProjectNameIndex = name.LastIndexOf(Application.productName, StringComparison.OrdinalIgnoreCase);
			if (unityProjectNameIndex > 0)
			{
				return name.Substring(unityProjectNameIndex);
			}
			return name;
		}
	}

	internal class NewExportModel
	{
		public string editor;
		public string editorVersion;
		public string userName;
		public string projectPath;
		public string projectName;
		public double buildDuration;
		public int totalFilesCount;

		/// <summary>
		/// in MB
		/// </summary>
		public float totalFilesSize;

		public string details;
		
		public string licenseEmail;
		public string licenseKey;

		public NewExportModel()
		{
			editor = "unity";
			editorVersion = Application.unityVersion;
#if UNITY_EDITOR
			userName = CloudProjectSettings.userName;
#endif
			if (userName == "anonymous" || string.IsNullOrWhiteSpace(userName))
				userName = AnalyticsHelper.GetUserName();
			licenseEmail = LicenseCheck.LicenseEmail;
			licenseKey = LicenseCheck.LicenseKey;
		}
	}
}