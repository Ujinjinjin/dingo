using Dingo.Cli.Repository.DbClasses;
using System;
using System.Threading.Tasks;

namespace Dingo.Cli.Repository
{
	internal interface IDatabaseContext: IDisposable
	{
		Task<DbSystemCheckTableExistenceResult> CheckTableExistenceAsync(string schema, string table);
		DbSystemCheckTableExistenceResult CheckTableExistence(string schema, string table);
	}
}