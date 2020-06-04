using Dingo.Cli.Extensions;
using System;
using System.IO;

namespace Dingo.Cli.Operations
{
	internal class PathHelper : IPathHelper
	{
		public string BuildFilePath(string path, string filename, string extension)
		{
			return path + filename + extension;
		}

		public string GetApplicationBaseDirectory()
		{
			return AppDomain.CurrentDomain.BaseDirectory
				.ReplaceBackslashesWithSlashes();
		}

		public string GetExecutionBaseDirectory()
		{
			return $"{Directory.GetCurrentDirectory()}/"
				.ReplaceBackslashesWithSlashes();
		}

		public string GetAbsolutePathFromRelative(string relativePath)
		{
			relativePath = relativePath
				.ReplaceBackslashesWithSlashes();

			return GetApplicationBaseDirectory() + relativePath;
		}
	}
}
