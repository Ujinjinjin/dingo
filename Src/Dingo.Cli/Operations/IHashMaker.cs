using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal interface IHashMaker
	{
		Task<string> GetFileHashAsync(string filename);
		string GetFileHash(string filename);
		Task<IDictionary<string, string>> GetFileListHashAsync(IList<string> filenameList);
		IDictionary<string, string> GetFileListHash(IList<string> filenameList);
	}
}
