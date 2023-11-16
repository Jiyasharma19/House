using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Needle.Engine.Core;
using Needle.Engine.Interfaces;
using Needle.Engine.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Needle.Engine.AdditionalData
{
	public class FogSettings : IBuildStageCallbacks
	{
		
		public Task<bool> OnBuild(BuildStage stage, ExportContext context)
		{
			if (stage == BuildStage.PreBuildScene)
			{
				var exportObject = ObjectUtils.FindObjectOfType<IExportableObject>() as Component;
				if (exportObject)
				{
					Create(exportObject.gameObject);
				}
			}
			else if (stage == BuildStage.PostBuildScene || stage == BuildStage.BuildFailed)
			{
				if (created.Count > 0)
				{
					foreach (var fog in created)
					{
						Object.DestroyImmediate(fog);
					}
					created.Clear();
				}
			}
			
			return Task.FromResult(true);
		}
	
		private static readonly List<Fog> created = new List<Fog>();
		
		internal static void Create(GameObject go)
		{
			try
			{
				if (go)
				{
					var fog = go.AddComponent<Fog>();
					created.Add(fog);
					fog.hideFlags = HideFlags.HideAndDontSave;
					fog.enabled = RenderSettings.fog;
					fog.mode = RenderSettings.fogMode;
					fog.color = RenderSettings.fogColor;
					fog.density = RenderSettings.fogDensity;
					fog.near = RenderSettings.fogStartDistance;
					fog.far = RenderSettings.fogEndDistance;
				}
			}
			catch(Exception ex)
			{
				// ignored
				Debug.LogWarning("Failed to export fog\n" + ex, go);
			}
		}
	}
}