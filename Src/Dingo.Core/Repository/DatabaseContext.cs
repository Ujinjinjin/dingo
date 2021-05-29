using Dingo.Core.Repository.DbClasses;
using Dingo.Core.Repository.DbConverters;
using Dingo.Core.Utils.Db;
using LinqToDB.Data;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dingo.Core.Repository
{
	internal sealed class DatabaseContext : DataConnectionBase, IDatabaseContext
	{
		private readonly IDatabaseContractConverter _databaseContractConverter;
		
		public DatabaseContext(
			string providerName,
			string connectionString,
			ILoggerFactory loggerFactory,
			IDatabaseContractConverter databaseContractConverter
		) : base(
			providerName,
			connectionString,
			loggerFactory?.CreateLogger<DatabaseContext>() ?? throw new ArgumentNullException(nameof(loggerFactory))
		)
		{
			_databaseContractConverter = databaseContractConverter ?? throw new ArgumentNullException(nameof(databaseContractConverter));
		}

		/// <inheritdoc />
		public async Task<DbDingoTableExistsResult> CheckTableExistenceAsync(string schema, string table)
		{
			var result = await QueryAsync<DbDingoTableExistsResult>(
				"dingo__table_exists",
				new DataParameter("p_table_name", table),
				new DataParameter("p_schema_name", schema)
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
				"dingo__get_migrations_status",
				_databaseContractConverter.ToDataParameter("pti_migration_info_input", dbMigrationInfoInputList)
			);
			return result.ToArray();
		}

		/// <inheritdoc />
		public Task HandshakeDatabaseConnectionAsync()
		{
			if (Connection.State == ConnectionState.Open)
			{
				return Task.CompletedTask;
			}
			
			Connection.Open();
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public async Task RegisterMigrationAsync(string migrationPath, string migrationHash, DateTime dateUpdated)
		{
			await ExecuteAsync(
				"dingo__register_migration",
				new DataParameter("p_migration_path", migrationPath),
				new DataParameter("p_migration_hash", migrationHash),
				new DataParameter("p_date_updated", dateUpdated)
			);
		}

		/// <inheritdoc />
		public Task ReloadDatabaseTypesAsync()
		{
			if (Connection is NpgsqlConnection npgsqlConnection)
			{
				npgsqlConnection.ReloadTypes();
			}

			return Task.CompletedTask;
		}
	}
}
