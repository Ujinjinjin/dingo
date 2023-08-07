namespace Dingo.Core.Adapters;

/// <inheritdoc />
internal sealed class PathAdapter : IPathAdapter
{
	private const char DirectorySeparatorChar = '/';

	/// <inheritdoc />
	public string CleanPath(string path)
	{
		return path
			.Replace('\\', DirectorySeparatorChar)
			.Replace('/', DirectorySeparatorChar)
			.Replace(Path.DirectorySeparatorChar, DirectorySeparatorChar)
			.Replace(Path.AltDirectorySeparatorChar, DirectorySeparatorChar);
	}

	/// <inheritdoc />
	public string GetFileName(string path)
	{
		return Path.GetFileName(path);
	}

	/// <inheritdoc />
	public string GetRelativePath(string relativeTo, string path)
	{
		return Path.GetRelativePath(relativeTo, path);
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
