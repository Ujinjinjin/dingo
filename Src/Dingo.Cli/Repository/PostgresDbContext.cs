using Dingo.Cli.DbUtils;
using Dingo.Cli.Loggers;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Dingo.Cli.Repository
{
	internal class PostgresDbContext : DataConnectionBase
	{
		private static readonly ILogger Logger = new ConsoleLogger();
		
		protected internal PostgresDbContext(string connectionString)
			: base(ProviderName.PostgreSQL95, connectionString, Logger)
		{
		}

		public async Task DummyAsync()
		{
			var result = Query<DbSystemCheckTableExistence>(
					"system__check_table_existence",
					new DataParameter("p_table_schema", "public"),
					new DataParameter("p_table_name", "dingo_script")
				)
				.Single();
		}

		private class DbSystemCheckTableExistence
		{
			[Column("system__check_table_existence")]
			public bool SystemCheckTableExistence { get; set; }
		}
	}
}
