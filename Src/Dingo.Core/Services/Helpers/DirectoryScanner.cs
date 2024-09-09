using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Services.Adapters;

namespace Dingo.Core.Services.Helpers;

/// <inheritdoc />
internal sealed class DirectoryScanner : IDirectoryScanner
{
	private readonly IDirectory _directory;
	private readonly IPath _path;

	public DirectoryScanner(
		IDirectory directory,
		IPath path
	)
	{
		_directory = directory.Required(nameof(directory));
		_path = path.Required(nameof(path));
	}

	/// <inheritdoc />
	public IReadOnlyList<MigrationPath> Scan(string rootPath, string searchPattern)
	{
		var fileList = _directory.GetFiles(rootPath, searchPattern, SearchOption.AllDirectories);

		var migrationPaths = new MigrationPath[fileList.Length];

		for (var i = 0; i < fileList.Length; i++)
		{
			var absolutePath = fileList[i];
			var relativePath = _path.GetRelativePath(rootPath, absolutePath);
			var filename = _path.GetFileName(absolutePath);

			migrationPaths[i] = new MigrationPath(
				absolutePath,
				relativePath,
				_path.GetRootDirectory(relativePath),
				filename
			);
		}

		return migrationPaths
			.OrderMigrations()
			.ToArray();
	}
}
