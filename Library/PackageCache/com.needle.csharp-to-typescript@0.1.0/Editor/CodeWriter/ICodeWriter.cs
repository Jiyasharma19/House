namespace Needle.Engine.Codegen
{
	public interface ICodeWriter
	{
		void BeginBlock();
		void EndBlock();
		void WriteLine(string str = null);
		
		void BeginLine();
		void Write(string str);
		void EndLine();
	}
}