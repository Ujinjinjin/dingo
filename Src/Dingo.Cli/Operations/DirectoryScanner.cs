using Dingo.Cli.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal class DirectoryScanner : IDirectoryScanner
	{
		public Task<IList<string>> GetFileListAsync(string rootPath, string searchPattern, bool absolutePath = true)
		{
			return Task.FromResult(GetFileList(rootPath, searchPattern, absolutePath));
		}
		
		public IList<string> GetFileList(string rootPath, string searchPattern, bool absolutePath = true)
		{
			var fileList = Directory.GetFiles(rootPath, searchPattern, SearchOption.AllDirectories)
				.OrderBy(x => x)
				.ToArray();
			for (var i = 0; i < fileList.Length; i++)
			{
				fileList[i] = fileList[i]
					.ReplaceBackslashesWithSlashes()
					.Replace(rootPath, absolutePath ? rootPath : "");
			}
			return fileList;
		}
	}
}
