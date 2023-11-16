using JetBrains.Annotations;
using Needle.Engine.Settings;

namespace Needle.Engine.Core
{
	[UsedImplicitly]
	internal class UseGizp : IBuildConfigProperty
	{
		public static bool Enabled
		{
			get => NeedleEngineBuildOptions.UseGzipCompression;
			set => NeedleEngineBuildOptions.UseGzipCompression = value;
		}
		
		public string Key => "gzip";
		public object GetValue(string projectDirectory)
		{
			return NeedleEngineBuildOptions.UseGzipCompression;
		}
	}


	[UsedImplicitly]
	internal class UseHotReload : IBuildConfigProperty
	{
		public string Key => "allowHotReload";
		public object GetValue(string projectDirectory)
		{
			return ExporterProjectSettings.instance.useHotReload;
		}
	}
}