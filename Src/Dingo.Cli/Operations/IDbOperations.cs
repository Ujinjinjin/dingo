using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal interface IDbOperations
	{
		Task<bool> CheckMigrationTableExistenceAsync();

		Task InstallDingoProceduresAsync();
	}
}
