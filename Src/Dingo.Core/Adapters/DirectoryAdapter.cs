using System.IO;

namespace Dingo.Core.Adapters
{
	public class DirectoryAdapter : IDirectoryAdapter
	{
		public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
			=> Directory.GetFiles(path, searchPattern, searchOption);
	}
}
