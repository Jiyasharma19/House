using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Needle.Engine.Codegen.Utils;

namespace Needle.Engine.Codegen
{
	public class MethodBuilder : ICodeBuilder
	{
		private readonly TypescriptBuilder builder;
		private readonly string methodName;
		private readonly ClassBuilder parent;

		private bool isPublic;
		private bool isProtected;
		private bool isStatic;
		private string returnType;
		private readonly List<(string name, string type)> parameters = new List<(string name, string type)>();

		private string csharpMethodString;

		public MethodBuilder(TypescriptBuilder builder, string methodName, ClassBuilder parent)
		{
			this.builder = builder;
			this.methodName = methodName;
			this.parent = parent;
		}

		public void PreBuild()
		{
		}

		public void Build(ICodeWriter writer)
		{
			if (!string.IsNullOrWhiteSpace(csharpMethodString))
				writer.WriteLine("// " + csharpMethodString);
			var str = "";
			var makePublic = isPublic;
			var finalMethodName = this.methodName.ToVariableName();

			switch (finalMethodName)
			{
				case "awake":
				case "start":
				case "onEnable":
				case "onDisable":
				case "update":
				case "onDestroy":
				case "onValidate":
					makePublic = true;
					break;
			}

			if (makePublic) str += "public ";
			else if (isProtected) str += "protected ";
			else str += "private ";
			if (isStatic) str += "static ";
			str += finalMethodName + "";
			str += "(";
			str += string.Join(", ", parameters.Select(p => p.name.ToVariableName() + " : " + p.type));
			str += ")";
			if (!string.IsNullOrEmpty(returnType)) str += " : " + returnType;
			writer.WriteLine(str);
			writer.BeginBlock();
			if(finalMethodName == "awake") writer.WriteLine("console.log(this);");
			else if(finalMethodName == "start" && !ClassHasAwakeMethod()) writer.WriteLine("console.log(this);");
			writer.WriteLine("// TODO: implement method body");
			writer.EndBlock();
			writer.WriteLine();
		}

		internal void Parse(MethodDeclarationSyntax node)
		{
			csharpMethodString = node.ToString().Split('\r', '\n').FirstOrDefault();
			if (csharpMethodString != null)
			{
				var arrowSyntax = csharpMethodString.IndexOf("=>", StringComparison.Ordinal);
				if (arrowSyntax > 0)
					csharpMethodString = csharpMethodString.Substring(0, arrowSyntax);
				var openBrace = csharpMethodString.IndexOf("{", StringComparison.Ordinal);
				if (openBrace > 0)
					csharpMethodString = csharpMethodString.Substring(0, openBrace);
			}

			isPublic = node.Modifiers.FirstOrDefault(m => m.Kind() == SyntaxKind.PublicKeyword) != default;
			isProtected = node.Modifiers.FirstOrDefault(m => m.Kind() == SyntaxKind.ProtectedKeyword) != default;
			isStatic = node.Modifiers.FirstOrDefault(m => m.Kind() == SyntaxKind.StaticKeyword) != default;
			returnType = SyntaxTypeHelper.Parse(node.ReturnType, out _, builder);
			if (string.IsNullOrWhiteSpace(returnType)) returnType = "void";
			var parameterList = node.ParameterList;
			if (parameterList != null)
			{
				foreach (var parameter in parameterList.Parameters)
				{
					var type = parameter.Type;
					var name = parameter.Identifier.Text;
					var typeName = SyntaxTypeHelper.Parse(type, out _, builder);
					parameters.Add((name, typeName));
				}
			}
		}
		
		private bool ClassHasStartMethod() => parent.Methods.Any(m => m.methodName == "Start");
		private bool ClassHasAwakeMethod() => parent.Methods.Any(m => m.methodName == "Awake");
	}
}