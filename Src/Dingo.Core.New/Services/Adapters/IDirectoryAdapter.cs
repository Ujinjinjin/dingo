namespace Dingo.Core.Services.Adapters;

/// <summary> Adapter for <see cref="System.IO.Directory"/> for unit testing purposes </summary>
public interface IDirectoryAdapter
{
	/// <inheritdoc cref="Directory.Exists(string?)"/>
	bool Exists(string path);

	/// <inheritdoc cref="Directory.CreateDirectory"/>
	DirectoryInfo CreateDirectory(string path);
		
	/// <inheritdoc cref="Directory.GetFiles(string, string, SearchOption)"/>
	string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
}