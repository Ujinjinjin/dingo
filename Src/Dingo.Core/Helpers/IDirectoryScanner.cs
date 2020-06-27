using Dingo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Core.Helpers
{
	/// <summary> Directory scanner </summary>
	internal interface IDirectoryScanner
	{
		/// <summary> Get all files found by pattern in specified folder and its subfolders </summary>
		/// <param name="rootPath">Folder, where files should be searched</param>
		/// <param name="searchPattern">Search pattern to be applied during search</param>
		/// <returns>List of <see cref="FilePath"/></returns>
		Task<IList<FilePath>> GetFilePathListAsync(string rootPath, string searchPattern);
		
		/// <summary> Get all files found by pattern in specified folder and its subfolders </summary>
		/// <param name="rootPath">Folder, where files should be searched</param>
		/// <param name="searchPattern">Search pattern to be applied during search</param>
		/// <returns>List of <see cref="FilePath"/></returns>
		IList<FilePath> GetFilePathList(string rootPath, string searchPattern);
	}
}
