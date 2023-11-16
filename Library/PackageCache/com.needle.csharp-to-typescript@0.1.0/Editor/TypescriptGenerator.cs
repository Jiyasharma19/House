using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using UnityEditor;
using File = System.IO.File;

namespace Needle.Engine.Codegen
{
	public class TypescriptGenerator
	{
		public string Run(string filePath, string targetDirectory)
		{
			var text = File.ReadAllText(filePath);
			var tree = CSharpSyntaxTree.ParseText(text);
			var root = tree.GetCompilationUnitRoot();

			// var compilation = CSharpCompilation.Create("HelloWorld")
			// 	.AddReferences( MetadataReference.CreateFromFile( typeof(object).Assembly.Location))
			// 	.AddSyntaxTrees(tree);
			// var model = compilation.GetSemanticModel(tree);
			// var reference = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
			
			var writer = new CodeStringWriter();
			var walker = new TypescriptWalker();
			walker.Run(root, writer);

			Directory.CreateDirectory(targetDirectory);
			var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
			var resultPath = targetDirectory + "/" + fileNameWithoutExtension + ".ts";
			File.WriteAllText(resultPath, writer.ToString());
			AssetDatabase.Refresh();
			return resultPath;
		}
	}

	// https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis

}