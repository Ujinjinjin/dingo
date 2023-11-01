using Dingo.Core.Models;

namespace Dingo.Core.Services.Helpers;

/// <summary> Directory scanner </summary>
internal interface IDirectoryScanner
{
	/// <summary> Get all files found by pattern in specified folder and its subfolders </summary>
	/// <param name="rootPath">Folder, where files should be searched</param>
	/// <param name="searchPattern">Search pattern to be applied during search</param>
	/// <returns>List of <see cref="MigrationPath"/></returns>
	IReadOnlyList<MigrationPath> Scan(string rootPath, string searchPattern);
}
