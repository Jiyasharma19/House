namespace Needle.Engine.Codegen
{
	public interface ICodeBuilder
	{
		void PreBuild();
		void Build(ICodeWriter writer);
	}
}