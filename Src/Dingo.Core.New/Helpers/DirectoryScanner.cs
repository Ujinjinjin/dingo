using Dingo.Core.Adapters;
using Dingo.Core.Extensions;
using Dingo.Core.Models;

namespace Dingo.Core.Helpers;

/// <inheritdoc />
internal sealed class DirectoryScanner : IDirectoryScanner
{
	private readonly IDirectoryAdapter _directoryAdapter;
	private readonly IPathAdapter _pathAdapter;

	public DirectoryScanner(
		IDirectoryAdapter directoryAdapter,
		IPathAdapter pathAdapter
	)
	{
		_directoryAdapter = directoryAdapter.Required(nameof(directoryAdapter));
		_pathAdapter = pathAdapter.Required(nameof(pathAdapter));
	}

	/// <inheritdoc />
	public IReadOnlyList<MigrationPath> Scan(string rootPath, string searchPattern)
	{
		var fileList = _directoryAdapter.GetFiles(rootPath, searchPattern, SearchOption.AllDirectories);

		var migrationPaths = new MigrationPath[fileList.Length];

		for (var i = 0; i < fileList.Length; i++)
		{
			var absolutePath = fileList[i];
			var relativePath = _pathAdapter.GetRelativePath(rootPath, absolutePath);
			var filename = _pathAdapter.GetFileName(absolutePath);

			migrationPaths[i] = new MigrationPath(
				absolutePath,
				relativePath,
				_pathAdapter.GetRootDirectory(relativePath),
				filename
			);
		}

		return migrationPaths
			.OrderBy(x => x.Module)
			.ThenBy(x => x.Filename)
			.ToArray();
	}
}
