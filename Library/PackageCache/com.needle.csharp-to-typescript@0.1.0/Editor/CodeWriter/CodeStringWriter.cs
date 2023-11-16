using System.IO;

namespace Needle.Engine.Codegen
{
	internal class CodeStringWriter : ICodeWriter
	{
		private int indentation;

		private readonly StringWriter builder = new StringWriter();

		public void BeginBlock()
		{
			WriteLine("{");
			indentation += 1;
		}

		public void EndBlock()
		{
			indentation -= 1;
			WriteLine("}");
		}

		public void WriteLine(string str)
		{
			BeginLine();
			Write(str);
			EndLine();
		}

		public void BeginLine()
		{
			for (var i = 0; i < indentation; i++)
				builder.Write("\t");
		}

		public void Write(string str)
		{
			if (str == null) return;
			this.builder.Write(str);
		}

		public void EndLine()
		{
			builder.Write("\n");
		}

		public override string ToString()
		{
			return builder.ToString();
		}
	}
}