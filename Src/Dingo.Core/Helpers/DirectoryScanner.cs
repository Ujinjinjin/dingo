using Dingo.Core.Extensions;
using Dingo.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dingo.Core.Helpers
{
	/// <inheritdoc />
	internal class DirectoryScanner : IDirectoryScanner
	{
		/// <inheritdoc />
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
