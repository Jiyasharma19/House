using System;
using System.Collections.Generic;

namespace Needle.Engine.Codegen
{
	public interface IImportHandler
	{
		void RegisterImport(string type, string source);
	}
	
	public class TypescriptBuilder : ICodeBuilder, IImportHandler
	{
		private readonly Dictionary<string, List<string>> imports = new Dictionary<string, List<string>>();
		private readonly List<ClassBuilder> @classes = new List<ClassBuilder>();
		private readonly Stack<ClassBuilder> classStack = new Stack<ClassBuilder>();

		public void RegisterImport(string type, string source)
		{
			if (imports.TryGetValue(source, out var list))
			{
				if (list.Contains(type)) return;
				list.Add(type);
			}
			else
			{
				imports.Add(source, new List<string> { type });
			}
		}

		public ClassBuilder StartClass(string name)
		{
			var classBuilder = new ClassBuilder(this, name);
			var currentClass = classStack.Count > 0 ? classStack.Peek() : null;
			currentClass?.AddMember(classBuilder);
			classes.Add(classBuilder);
			classStack.Push(classBuilder);
			return classBuilder;
		}

		public void EndClass() => classStack.Pop();

		/// <summary>
		/// Tries to create a new field, returns null if no class is currently active
		/// </summary>
		public FieldBuilder AddField(string fieldName)
		{
			var currentClass = classStack.Count > 0 ? classStack.Peek() : null;
			if (currentClass == null) return null;
			var fieldBuilder = new FieldBuilder(this, fieldName);
			currentClass.AddMember(fieldBuilder);
			return fieldBuilder;
		}

		public MethodBuilder AddMethod(string methodName)
		{
			var currentClass = classStack.Count > 0 ? classStack.Peek() : null;
			if (currentClass == null) return null;
			var methodBuilder = new MethodBuilder(this, methodName, currentClass);
			currentClass.AddMember(methodBuilder);
			return methodBuilder;
		}

		public void PreBuild()
		{
			foreach(var cl in classes) cl.PreBuild();
		}

		public void Build(ICodeWriter writer)
		{
			foreach (var import in imports)
			{
				writer.WriteLine("import { " + string.Join(", ", import.Value) + " } from \"" + import.Key + "\";");
			}
			if(imports.Count > 0) writer.WriteLine();

			foreach (var @class in classes)
			{
				writer.WriteLine("// @dont-generate-component");
				@class.Build(writer);
			}
		}
	}
}