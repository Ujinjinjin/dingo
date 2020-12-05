using Dingo.Core.Adapters;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dingo.Core.Helpers
{
	/// <inheritdoc />
	internal class DirectoryScanner : IDirectoryScanner
	{
		private readonly IDirectoryAdapter _directoryAdapter;
		private readonly IPathHelper _pathHelper;

		public DirectoryScanner(IDirectoryAdapter directoryAdapter, IPathHelper pathHelper)
		{
			_directoryAdapter = directoryAdapter ?? throw new ArgumentNullException(nameof(directoryAdapter));
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		}

		/// <inheritdoc />
		public IList<FilePath> GetFilePathList(string rootPath, string searchPattern)
		{
			var fileList = _directoryAdapter.GetFiles(rootPath, searchPattern, SearchOption.AllDirectories);

			var filePathList = new FilePath[fileList.Length];

			for (var i = 0; i < fileList.Length; i++)
			{
				var absolutePath = fileList[i].ReplaceBackslashesWithSlashes();
				var relativePath = absolutePath.Replace(rootPath, string.Empty);
				filePathList[i] = new FilePath
				{
					Absolute = absolutePath,
					Relative = relativePath,
					Filename = Path.GetFileName(absolutePath),
					Module = _pathHelper.GetRootDirectory(relativePath)
				};
			}

			return filePathList
				.OrderBy(x => x.Module)
				.ThenBy(x => x.Filename)
				.ToArray();
		}
	}
}
