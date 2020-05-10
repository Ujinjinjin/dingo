using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal interface IDirectoryScanner
	{
		Task<IList<string>> GetFileListAsync(string rootPath, string searchPattern, bool absolutePath = true);
		IList<string> GetFileList(string rootPath, string searchPattern, bool absolutePath = true);
	}
}
