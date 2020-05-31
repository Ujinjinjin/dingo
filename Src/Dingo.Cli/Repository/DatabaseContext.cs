using Dingo.Cli.DbUtils;
using Dingo.Cli.Loggers;
using Dingo.Cli.Repository.DbClasses;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dingo.Cli.Repository
{
	internal class DatabaseContext : DataConnectionBase, IDatabaseContext
	{
		private static readonly ILogger Logger = new ConsoleLogger();
		
		protected internal DatabaseContext(
			string providerName,
			string connectionString
		) : base(providerName, connectionString, Logger)
		{
		}

		public async Task ExecuteRawSqlAsync(string sql)
		{
			await ExecuteSqlAsync(sql);
		}

		public Task<DbSystemCheckTableExistenceResult> CheckTableExistenceAsync(string schema, string table)
		{
			return Task.FromResult(CheckTableExistence(schema, table));
		}

		public DbSystemCheckTableExistenceResult CheckTableExistence(string schema, string table)
		{
			return Query<DbSystemCheckTableExistenceResult>(
					"system__check_table_existence",
					new DataParameter("p_table_schema", schema),
					new DataParameter("p_table_name", table)
				)
				.Single();
		}

		public async Task RegisterMigrationAsync(string migrationPath, string migrationHash, DateTime dateUpdated)
		{
			await ExecuteAsync(
				"system__register_migration",
				new DataParameter("p_migration_path", migrationPath),
				new DataParameter("p_migration_hash", migrationHash),
				new DataParameter("p_date_updated", dateUpdated)
			);
		}
	}
}
