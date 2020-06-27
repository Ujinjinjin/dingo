using Dingo.Core.Extensions;
using System;
using System.IO;

namespace Dingo.Core.Helpers
{
	/// <inheritdoc />
	internal class PathHelper : IPathHelper
	{
		/// <inheritdoc />
		public string BuildFilePath(string path, string filename, string extension)
		{
			return path + filename + extension;
		}

		/// <inheritdoc />
		public string GetApplicationBaseDirectory()
		{
			return AppDomain.CurrentDomain.BaseDirectory
				.ReplaceBackslashesWithSlashes();
		}

		/// <inheritdoc />
		public string GetExecutionBaseDirectory()
		{
			return $"{Directory.GetCurrentDirectory()}/"
				.ReplaceBackslashesWithSlashes();
		}

		/// <inheritdoc />
		public string GetAbsolutePathFromRelative(string relativePath)
		{
			relativePath = relativePath
				.ReplaceBackslashesWithSlashes();

			return GetExecutionBaseDirectory() + relativePath;
		}

		/// <inheritdoc />
		public string GetAppRootPathFromRelative(string relativePath)
		{
			relativePath = relativePath
				.ReplaceBackslashesWithSlashes();

			return GetApplicationBaseDirectory() + relativePath;
		}
	}
}
