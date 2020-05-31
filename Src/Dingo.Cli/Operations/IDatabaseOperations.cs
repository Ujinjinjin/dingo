using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal interface IDatabaseOperations
	{
		Task<bool> CheckMigrationTableExistenceAsync();

		Task InstallCheckTableExistenceProcedureAsync();

		Task ApplyMigrationAsync(string sql, string path, string migrationHash, bool silent = false);
		
		Task RegisterMigrationAsync(string migrationPath, string migrationHash);
	}
}
