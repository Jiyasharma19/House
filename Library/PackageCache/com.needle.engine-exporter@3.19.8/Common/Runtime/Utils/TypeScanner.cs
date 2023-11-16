using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Needle.Engine.Utils
{
	public static class TypeScanner
	{
		// private static readonly Dictionary<(string, SearchOption), IList<ImportInfo>> cache = new Dictionary<(string, SearchOption), IList<ImportInfo>>();
		//
		// public static void ClearCache(string directory)
		// {
		// 	
		// }

		public static void FindTypes(string directory, IList<ImportInfo> copyTo, SearchOption option = SearchOption.AllDirectories)
		{
			if (directory.StartsWith("http")) return;
			if (!Directory.Exists(directory)) return;

			// if (cache.TryGetValue((directory, option), out var list))
			// {
			// 	foreach (var e in list)
			// 		copyTo.Add(e);
			// 	return;
			// }

			foreach (var file in Directory.EnumerateFiles(directory, "*.*", option))
			{
				FindTypesInFile(file, copyTo);
			}
			
			// cache[(directory, option)] = copyTo;
		}
		
		public static void FindTypesExcludingNodeModules(string directory, IList<ImportInfo> copyTo)
		{
			if (!Directory.Exists(directory))
				return;
			
			foreach (var file in Directory.EnumerateFiles(directory, "*.*", SearchOption.TopDirectoryOnly))
				FindTypesInFile(file, copyTo);
			
			var dirs = Directory.EnumerateDirectories(directory);
			foreach (var dir in dirs)
			{
				if (dir.EndsWith("node_modules")) continue;
				FindTypes(dir, copyTo);
			}
		}
		
		// TODO: change this to only find types that extend a base class for codegen (but for generating the component export file we still want to capture all classes) 
		private static readonly Regex findClass = new Regex("export class (?<class>.+?) ");

		public static void FindTypesInFile(string filePath, IList<ImportInfo> list)
		{
			var fi = new FileInfo(filePath);
			var fileName = fi.Name;
			if (fileName.StartsWith("__")) return;
			var validFile = fileName.EndsWith(".js") || fileName.EndsWith(".ts");
			if (!validFile) return;
			var content = File.ReadAllText(filePath);
			var classes = findClass.Matches(content);
			var packageJson = FindPackageJson(fi.Directory);
			foreach (Match match in classes)
			{
				if (match.Success)
				{
					var isCommentedOut = IsCommentedOut(content, match.Index);
					if (isCommentedOut) continue;
					var importName = match.Groups["class"].Value;
					// var start = match.Index;
					// var end = content.IndexOf("{", start, StringComparison.Ordinal);
					// var line = content.Substring(start, end - start);
					var imp = new ImportInfo(importName, filePath, content, packageJson);
					list.Add(imp);
				}
			}
		}

		private static bool IsCommentedOut(string content, int index)
		{
			for (var i = index; i > 0; i--)
			{
				var c = content[i];
				if (c == '\n') break;
				if (c == '/' && content[i - 1] == '/')
				{
					return true;
				}
			}
			return false;
		}

		private static FileInfo FindPackageJson(DirectoryInfo dir)
		{
			while (true)
			{
				var packageJsonPath = new FileInfo(dir.FullName + "/package.json");
				if (packageJsonPath.Exists) return packageJsonPath;
				if (dir.Parent != null) dir = dir.Parent;
				else
					return null;
			}
		}
	}
}