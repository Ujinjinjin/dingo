using Dingo.Core.Repository.DbClasses;
using Dingo.Core.Repository.DbConverters;
using Dingo.Core.Utils.Db;
using LinqToDB.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dingo.Core.Repository
{
	internal class DatabaseContext : DataConnectionBase, IDatabaseContext
	{
		private readonly IDatabaseContractConverter _databaseContractConverter;
		
		protected internal DatabaseContext(
			string providerName,
			string connectionString,
			ILoggerFactory loggerFactory,
			IDatabaseContractConverter databaseContractConverter
		) : base(
			providerName,
			connectionString,
			loggerFactory?.CreateLogger<DatabaseContext>()
		)
		{
			_databaseContractConverter = databaseContractConverter ?? throw new ArgumentNullException(nameof(databaseContractConverter));
		}

		/// <inheritdoc />
		public async Task<DbSystemCheckTableExistenceResult> CheckTableExistenceAsync(string schema, string table)
		{
			var result = await QueryAsync<DbSystemCheckTableExistenceResult>(
				"system__check_table_existence",
				new DataParameter("p_table_schema", schema),
				new DataParameter("p_table_name", table)
			);
			return result.Single();
		}

		/// <inheritdoc />
		public async Task ExecuteRawSqlAsync(string sql)
		{
			await ExecuteSqlAsync(sql);
		}

		/// <inheritdoc />
		public async Task<IList<DbMigrationInfoOutput>> GetMigrationsStatusAsync(IList<DbMigrationInfoInput> dbMigrationInfoInputList)
		{
			var result = await QueryAsync<DbMigrationInfoOutput>(
				"system__get_migrations_status",
				_databaseContractConverter.ToDataParameter("pti_migration_info_input", dbMigrationInfoInputList)
			);
			return result.ToArray();
		}

		/// <inheritdoc />
		public Task HandshakeDatabaseConnectionAsync()
		{
			if (this.Connection.State == ConnectionState.Open)
			{
				return Task.CompletedTask;
			}
			
			this.Connection.Open();
			return Task.CompletedTask;
		}

		/// <inheritdoc />
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
