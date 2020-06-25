using Dingo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Core.Helpers
{
	internal interface IDatabaseHelper
	{
		Task<bool> CheckMigrationTableExistenceAsync();

		Task InstallCheckTableExistenceProcedureAsync();

		Task ApplyMigrationAsync(string sql, string path, string migrationHash, bool silent = false);
		
		Task RegisterMigrationAsync(string migrationPath, string migrationHash);

		Task<IList<MigrationInfo>> GetMigrationsStatusAsync(IList<MigrationInfo> migrationInfoList);
	}
}
