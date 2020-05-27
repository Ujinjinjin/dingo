using Dingo.Cli.Factories;
using Dingo.Cli.Repository;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal class PostgresOperations : IDbOperations
	{
		private readonly IPathHelper _pathHelper;
		private readonly IConfiguration _configuration;
		private readonly IDatabaseContextFactory _databaseContextFactory;

		public PostgresOperations(IPathHelper pathHelper, IConfiguration configuration, IDatabaseContextFactory databaseContextFactory)
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

		public async Task InstallDingoProceduresAsync()
		{
			var sqlScriptPath = _pathHelper.GetAbsolutePathFromRelative(_configuration.CheckTableExistenceProcedureFilePath);
			
			var sqlScriptText = await File.ReadAllTextAsync(sqlScriptPath);

			using (var dbContext = _databaseContextFactory.CreateDatabaseContext(_configuration.ProviderName, _configuration.ConnectionString))
			{
				await dbContext.ExecuteRawSqlAsync(sqlScriptText);
			}
		}
	}
}
