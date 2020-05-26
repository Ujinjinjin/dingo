using Dingo.Cli.DbUtils;
using Dingo.Cli.Loggers;
using Dingo.Cli.Repository.DbClasses;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Dingo.Cli.Repository
{
	internal class PostgresDbContext : DataConnectionBase, IDatabaseContext
	{
		private static readonly ILogger Logger = new ConsoleLogger();
		
		protected internal PostgresDbContext(string connectionString)
			: base(ProviderName.PostgreSQL95, connectionString, Logger)
		{
		}

		public Task<DbSystemCheckTableExistenceResult> CheckTableExistenceAsync(string schema, string table)
		{
			return Task.FromResult(CheckTableExistence(schema, table));
		}

		public DbSystemCheckTableExistenceResult CheckTableExistence(string schema, string table)
		{
			return Query<DbSystemCheckTableExistenceResult>(
					"system__check_table_existence",
					new DataParameter("p_table_schema", "public"),
					new DataParameter("p_table_name", "dingo_script")
				)
				.Single();
		}
	}
}
