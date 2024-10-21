namespace Dingo.Core.Services.Adapters;

/// <inheritdoc />
internal sealed class PathAdapter : IPath
{
	private const char DirectorySeparatorChar = '/';

	/// <inheritdoc />
	public string CleanPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return path;
		}

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
		return CleanPath(Path.GetRelativePath(relativeTo, path));
	}

	/// <inheritdoc />
	public string GetAbsolutePath(string relativePath)
	{
		var root = Directory.GetCurrentDirectory();
		var absolute = Join(root, relativePath);

		return CleanPath(absolute);
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

	/// <inheritdoc />
	public string GetAppDataPath()
	{
		if (OperatingSystem.IsLinux())
		{
			return Constants.AppDataLinux;
		}

		return CleanPath(AppDomain.CurrentDomain.BaseDirectory);
	}

	/// <inheritdoc />
	public string GetLogsPath()
	{
		return Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Constants.LogsDir);
	}

	/// <inheritdoc />
	public string Join(params string?[] paths)
	{
		return Path.Join(paths);
	}
}
