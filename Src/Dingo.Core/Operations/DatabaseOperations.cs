using Dingo.Core.Config;
using Dingo.Core.Factories;
using Dingo.Core.Models;
using Dingo.Core.Repository.DbClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dingo.Core.Operations
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

		public async Task<IList<MigrationInfo>> GetMigrationsStatusAsync(IList<MigrationInfo> migrationInfoList)
		{
			using (var dbContext = _databaseContextFactory.CreateDatabaseContext(_configuration.ProviderName, _configuration.ConnectionString))
			{
				var input = migrationInfoList
					.Select(x => new DbMigrationInfoInput
					{
						MigrationHash = x.NewHash,
						MigrationPath = x.Path.Relative
					})
					.ToArray();
				var dbMigrationStatuses = await dbContext.GetMigrationsStatusAsync(input);
				
				var result = new MigrationInfo[dbMigrationStatuses.Count];
				for (var i = 0; i < dbMigrationStatuses.Count; i++)
				{
					result[i] = new MigrationInfo
					{
						Path = new FilePath
						{
							Relative = dbMigrationStatuses[i].MigrationPath,
							Absolute = _pathHelper.GetAbsolutePathFromRelative(dbMigrationStatuses[i].MigrationPath)
						},
						NewHash = dbMigrationStatuses[i].NewHash,
						OldHash = dbMigrationStatuses[i].OldHash,
						Action = dbMigrationStatuses[i].IsOutdated switch
						{
							null => MigrationAction.Install,
							true => MigrationAction.Update,
							false => MigrationAction.Skip,
						}
					};
				}

				return result;
			}
		}
	}
}
