namespace Needle.Engine.Codegen.Utils
{
	internal static class StringUtils
	{
		public static string ToVariableName(this string str)
		{
			if (string.IsNullOrEmpty(str)) return str;
			if(str.StartsWith("m_"))
				str = str.Substring(2);
			if(char.IsUpper(str[0]))
				return char.ToLower(str[0]) + str.Substring(1);
			return str;
		}
	}
}