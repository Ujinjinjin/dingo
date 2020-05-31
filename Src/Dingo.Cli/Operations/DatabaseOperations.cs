using Dingo.Cli.Factories;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal class DatabaseOperations : IDatabaseOperations
	{
		private readonly IPathHelper _pathHelper;
		private readonly IConfiguration _configuration;
		private readonly IDatabaseContextFactory _databaseContextFactory;

		public DatabaseOperations(IPathHelper pathHelper, IConfiguration configuration, IDatabaseContextFactory databaseContextFactory)
		{
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_databaseContextFactory = databaseContextFactory ?? throw new ArgumentNullException(nameof(databaseContextFactory));
		}

		public async Task<bool> CheckMigrationTableExistenceAsync()
		{
			using (var dbContext = _databaseContextFactory.CreateDatabaseContext(_configuration.ProviderName, _configuration.ConnectionString))
			{
				var result = await dbContext.CheckTableExistenceAsync(_configuration.MigrationSchema, _configuration.MigrationTable);
				return result.SystemCheckTableExistence;
			}
		}

		public async Task InstallCheckTableExistenceProcedureAsync()
		{
			var sqlScriptPath = _pathHelper.GetAbsolutePathFromRelative(_configuration.CheckTableExistenceProcedurePath);
			var sqlScriptText = await File.ReadAllTextAsync(sqlScriptPath);

			using (var dbContext = _databaseContextFactory.CreateDatabaseContext(_configuration.ProviderName, _configuration.ConnectionString))
			{
				await dbContext.ExecuteRawSqlAsync(sqlScriptText);
			}
		}

		public async Task ApplyMigrationAsync(string sql, string migrationPath, string migrationHash, bool silent = false)
		{
			using (var dbContext = _databaseContextFactory.CreateDatabaseContext(_configuration.ProviderName, _configuration.ConnectionString))
			{
				await dbContext.ExecuteRawSqlAsync(sql);

				if (!silent)
				{
					await dbContext.RegisterMigrationAsync(migrationPath, migrationHash, DateTime.UtcNow);
				}
			}
		}

		public async Task RegisterMigrationAsync(string migrationPath, string migrationHash)
		{
			using (var dbContext = _databaseContextFactory.CreateDatabaseContext(_configuration.ProviderName, _configuration.ConnectionString))
			{
				await dbContext.RegisterMigrationAsync(migrationPath, migrationHash, DateTime.UtcNow);
			}
		}
	}
}
