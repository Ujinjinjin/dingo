using Dingo.Cli.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal interface IHashMaker
	{
		Task<string> GetFileHashAsync(string filename);
		string GetFileHash(string filename);
		Task<IList<MigrationInfo>> GetMigrationInfoListAsync(IList<FilePath> filePathList);
		IList<MigrationInfo> GetMigrationInfoList(IList<FilePath> filePathList);
	}
}
