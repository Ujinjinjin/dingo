namespace Dingo.Core.Services.Adapters;

/// <inheritdoc />
public sealed class DirectoryAdapter : IDirectory
{
	/// <inheritdoc />
	public bool Exists(string path) => Directory.Exists(path);

	/// <inheritdoc />
	public DirectoryInfo CreateDirectory(string path) => Directory.CreateDirectory(path);

	/// <inheritdoc />
	public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		=> Directory.GetFiles(path, searchPattern, searchOption);
}
