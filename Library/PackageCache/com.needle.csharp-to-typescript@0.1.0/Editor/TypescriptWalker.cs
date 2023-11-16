using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;

namespace Needle.Engine.Codegen
{
	internal class TypescriptWalker : CSharpSyntaxWalker
	{
		private readonly TypescriptBuilder builder;

		public TypescriptWalker()
		{
			this.builder = new TypescriptBuilder();
		}

		public void Run(CompilationUnitSyntax unit, ICodeWriter writer)
		{
			this.Visit(unit);
			this.builder.PreBuild();
			this.builder.Build(writer);
		}

		public override void VisitClassDeclaration(ClassDeclarationSyntax node)
		{
			var isComponent = node.BaseList?.Types.Any(t => t.Type.ToString() == "MonoBehaviour") ?? false;
			if (isComponent)
			{
				this.builder.RegisterImport("Behaviour", "@needle-tools/engine");
			}
			
			var publicKeyword = node.Modifiers.FirstOrDefault(m => m.Kind() == SyntaxKind.PublicKeyword);
			var name = node.Identifier.Text;
			
			var cl = this.builder.StartClass(name);
			cl.BaseType = isComponent ? "Behaviour" : "";
			cl.IsPublic = publicKeyword != default;
			base.VisitClassDeclaration(node);
			this.builder.EndClass();
		}

		public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
		{
			var isPublic = node.Modifiers.FirstOrDefault(m => m.Kind() == SyntaxKind.PublicKeyword);
			var isSerialized = node.AttributeLists.Any(a => a.Attributes.Any(attr => attr.Name.ToString() == "SerializeField"));
			if(isPublic == default && !isSerialized) return;
			
			var declaration = node.Declaration;
			foreach (var var in declaration.Variables)
			{
				var name = var.Identifier.Text;
				var field = builder.AddField(name);
				if (field != null)
				{
					field.isSerialized = isSerialized;
					field?.Parse(node);
				}

			}
			base.VisitFieldDeclaration(node);
		}

		public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
		{
			var name = node.Identifier.Text;
			var method = builder.AddMethod(name);
			method?.Parse(node);
			base.VisitMethodDeclaration(node);
		}
	}
}