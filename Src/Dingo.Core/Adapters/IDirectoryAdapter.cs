using System.IO;

namespace Dingo.Core.Adapters
{
	public interface IDirectoryAdapter
	{
		string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
	}
}
