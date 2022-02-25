using Dingo.Core.Extensions;
using System;
using System.IO;

namespace Dingo.Core.Helpers;

/// <inheritdoc />
internal sealed class PathHelper : IPathHelper
{
	/// <inheritdoc />
	public string BuildFilePath(string path, string filename, string extension)
	{
		return path + filename + extension;
	}

	/// <inheritdoc />
	public string GetAbsolutePathFromRelative(string relativePath)
	{
		return GetExecutionBaseDirectory().ConcatPath(relativePath);
	}

	/// <inheritdoc />
	public string GetApplicationBaseDirectory()
	{
		return AppDomain.CurrentDomain.BaseDirectory
			.ReplaceBackslashesWithSlashes();
	}

	/// <inheritdoc />
	public string GetAppRootPathFromRelative(string relativePath)
	{
		return GetApplicationBaseDirectory().ConcatPath(relativePath);
	}

	/// <inheritdoc />
	public string GetExecutionBaseDirectory()
	{
		return $"{Directory.GetCurrentDirectory()}/"
			.ReplaceBackslashesWithSlashes();
	}

	/// <inheritdoc />
	public string GetLogsDirectory()
	{
		return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ConcatPath(".dingo/logs");
	}

	/// <inheritdoc />
	public string GetRootDirectory(string path)
	{
		while (true)
		{
			var temp = Path.GetDirectoryName(path);
			if (string.IsNullOrEmpty(temp))
				break;
			path = temp;
		}
		return path;
	}
}