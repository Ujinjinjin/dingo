using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal interface IDbOperations
	{
		Task<bool> CheckSystemTableExistence();

		Task InstallDingoProcedures();
	}
}
