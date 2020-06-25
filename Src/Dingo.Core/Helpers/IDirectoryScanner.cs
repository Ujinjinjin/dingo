using Dingo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Core.Helpers
{
	internal interface IDirectoryScanner
	{
		Task<IList<FilePath>> GetFilePathListAsync(string rootPath, string searchPattern);
		IList<FilePath> GetFilePathList(string rootPath, string searchPattern);
	}
}
