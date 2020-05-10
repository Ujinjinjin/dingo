using System.Collections.Generic;
using System.IO;
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
			var fileList = Directory.GetFiles(rootPath, searchPattern, SearchOption.AllDirectories);
			for (var i = 0; i < fileList.Length; i++)
			{
				fileList[i] = fileList[i]
					.Replace("\\", "/")
					.Replace(rootPath, absolutePath ? rootPath : "");
			}
			return fileList;
		}
	}
}
