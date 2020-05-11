using Dingo.Cli.Extensions;
using System;

namespace Dingo.Cli.Operations
{
	internal class PathHelper : IPathHelper
	{
		public string GetApplicationBasePath()
		{
			return AppDomain.CurrentDomain.BaseDirectory
				.ReplaceBackslashesWithSlashes();
		}

		public string GetAbsolutePathFromRelative(string relativePath)
		{
			relativePath = relativePath
				.ReplaceBackslashesWithSlashes();

			return GetApplicationBasePath() + relativePath;
		}
	}
}
