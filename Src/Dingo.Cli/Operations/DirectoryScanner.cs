using Dingo.Cli.Extensions;
using Dingo.Cli.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal class DirectoryScanner : IDirectoryScanner
	{
		public Task<IList<FilePath>> GetFilePathListAsync(string rootPath, string searchPattern)
		{
			return Task.FromResult(GetFilePathList(rootPath, searchPattern));
		}
		
		public IList<FilePath> GetFilePathList(string rootPath, string searchPattern)
		{
			var fileList = Directory.GetFiles(rootPath, searchPattern, SearchOption.AllDirectories)
				.OrderBy(x => x)
				.ToArray();
			
			var filePathList = new FilePath[fileList.Length];
			
			for (var i = 0; i < fileList.Length; i++)
			{
				var filePath = fileList[i].ReplaceBackslashesWithSlashes();
				filePathList[i] = new FilePath
				{
					Absolute = filePath,
					Relative = filePath.Replace(rootPath, string.Empty)
				};
			}
			return filePathList;
		}
	}
}
