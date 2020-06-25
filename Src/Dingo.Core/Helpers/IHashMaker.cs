using Dingo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Core.Helpers
{
	internal interface IHashMaker
	{
		Task<string> GetFileHashAsync(string filename);
		string GetFileHash(string filename);
		Task<IList<MigrationInfo>> GetMigrationInfoListAsync(IList<FilePath> filePathList);
		IList<MigrationInfo> GetMigrationInfoList(IList<FilePath> filePathList);
	}
}
