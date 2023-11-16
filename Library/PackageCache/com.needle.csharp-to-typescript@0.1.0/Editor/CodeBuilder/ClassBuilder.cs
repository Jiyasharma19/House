using System.Collections.Generic;
using System.Linq;

namespace Needle.Engine.Codegen
{
	public class ClassBuilder : ICodeBuilder
	{
		private readonly TypescriptBuilder builder;
		
		public readonly string Name;
		public string BaseType;
		public bool IsPublic;
		public readonly List<ICodeBuilder> Members = new List<ICodeBuilder>();

		public ClassBuilder(TypescriptBuilder builder, string name)
		{
			this.builder = builder;
			this.Name = name;
		}

		public void AddMember(ICodeBuilder child)
		{
			Members.Add(child);
		}

		public void PreBuild()
		{
			foreach(var member in Members) member.PreBuild();
		}

		public void Build(ICodeWriter writer)
		{
			var str = "";
			if (IsPublic) str += "export";
			str += " class " + Name;
			if (!string.IsNullOrEmpty(BaseType)) str += " extends " + BaseType;
			writer.WriteLine(str);
			writer.BeginBlock();
			foreach (var member in Members)
			{
				if (member is FieldBuilder field)
				{
					if (!string.IsNullOrEmpty(field.SerializableType))
					{
						
					}
				}
				member.Build(writer);
			}
			writer.EndBlock();
		}
		
		internal IEnumerable<MethodBuilder> Methods => Members.OfType<MethodBuilder>();
	}
}