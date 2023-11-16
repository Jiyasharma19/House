using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Needle.Engine.Codegen.Utils;

namespace Needle.Engine.Codegen
{
	public class FieldBuilder : ICodeBuilder
	{
		public string SerializableType => serializableType;

		private readonly TypescriptBuilder builder;
		private readonly string fieldName;

		private bool isPublic;
		private bool isArray;
		private string typeName;
		private string serializableType;
		private bool isPrimitiveType;
		private bool isStatic;
		public bool isSerialized;

		public FieldBuilder(TypescriptBuilder builder, string fieldName)
		{
			this.builder = builder;
			this.fieldName = fieldName;
		}

		internal void Parse(FieldDeclarationSyntax field)
		{
			var declaration = field.Declaration;
			var type = declaration.Type;
			isPrimitiveType = type is PredefinedTypeSyntax;
			isStatic = field.Modifiers.Any(SyntaxKind.StaticKeyword);
			typeName = SyntaxTypeHelper.Parse(type, out serializableType, builder);
		}

		public void PreBuild()
		{
			this.builder.RegisterImport("serializable", "@needle-tools/engine");
		}

		public void Build(ICodeWriter writer)
		{
			var isValid = !string.IsNullOrEmpty(typeName);
			if (!isValid) writer.WriteLine("/*");
			writer.WriteLine($"@serializable({serializableType})");
			var str = fieldName.ToVariableName();
			// if (isPrimitiveType) str += "!";
			str += ": " + typeName;
			if (!isPrimitiveType) str += " | null = null";
			else
			{
				if (typeName == "string") str += " = \"\"";
				else if (typeName == "boolean") str += " = false";
				else if (typeName == "number") str += " = 0";
				else if(typeName == "BigInt") str += " = BigInt(0)";
			}
			str += ";";
			if (isStatic) str = "static " + str;
			if (isValid) str += "\n";
			writer.WriteLine(str);
			if (!isValid) writer.WriteLine("*/\n");
		}
	}
}