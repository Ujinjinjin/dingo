using Dingo.Core.Adapters;
using Dingo.Core.Config;
using Dingo.Core.Factories;
using Dingo.Core.Helpers;
using Dingo.Core.Models;
using Dingo.Core.Repository.DbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dingo.Core.Repository
{
	/// <inheritdoc />
	internal class DatabaseRepository : IDatabaseRepository
	{
		private readonly IPathHelper _pathHelper;
		private readonly IConfigWrapper _configWrapper;
		private readonly IFileAdapter _fileAdapter;
		private readonly IDatabaseContextFactory _databaseContextFactory;

		public DatabaseRepository(
			IPathHelper pathHelper,
			IConfigWrapper configWrapper,
			IFileAdapter fileAdapter,
			IDatabaseContextFactory databaseContextFactory
		)
		{
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
			_fileAdapter = fileAdapter ?? throw new ArgumentNullException(nameof(fileAdapter));
			_databaseContextFactory = databaseContextFactory ?? throw new ArgumentNullException(nameof(databaseContextFactory));
		}

		/// <inheritdoc />
		public async Task ApplyMigrationAsync(string sql, string migrationPath, string migrationHash, bool registerMigrations = true)
		{
			using (var dbContext = _databaseContextFactory.CreateDatabaseContext())
			{
				await dbContext.ExecuteRawSqlAsync(sql);

				if (registerMigrations)
				{
					await dbContext.RegisterMigrationAsync(migrationPath, migrationHash, DateTime.UtcNow);
				}
			}
		}

		/// <inheritdoc />
		public async Task<bool> CheckMigrationTableExistenceAsync()
		{
			using (var dbContext = _databaseContextFactory.CreateDatabaseContext())
			{
				var result = await dbContext.CheckTableExistenceAsync(_configWrapper.MigrationSchema, _configWrapper.MigrationTable);
				return result.SystemCheckTableExistence;
			}
		}

		/// <inheritdoc />
		public async Task<IList<MigrationInfo>> GetMigrationsStatusAsync(IList<MigrationInfo> migrationInfoList)
		{
			using (var dbContext = _databaseContextFactory.CreateDatabaseContext())
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
							Absolute = _pathHelper.GetAppRootPathFromRelative(dbMigrationStatuses[i].MigrationPath)
						},
						NewHash = dbMigrationStatuses[i].NewHash,
						OldHash = dbMigrationStatuses[i].OldHash,
						Status = dbMigrationStatuses[i].IsOutdated switch
						{
							null => MigrationStatus.New,
							true => MigrationStatus.Outdated,
							false => MigrationStatus.UpToDate,
						}
					};
				}

				return result;
			}
		}

		/// <inheritdoc />
		public async Task<bool> HandshakeDatabaseConnectionAsync()
		{
			try
			{
				await InstallCheckTableExistenceProcedureAsync();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		/// <inheritdoc />
		public async Task InstallCheckTableExistenceProcedureAsync()
		{
			var sqlScriptPath = _pathHelper.GetAppRootPathFromRelative(_configWrapper.CheckTableExistenceProcedurePath);
			var sqlScriptText = await _fileAdapter.ReadAllTextAsync(sqlScriptPath);

			using (var dbContext = _databaseContextFactory.CreateDatabaseContext())
			{
				await dbContext.ExecuteRawSqlAsync(sqlScriptText);
			}
		}

		/// <inheritdoc />
		public async Task RegisterMigrationAsync(string migrationPath, string migrationHash)
		{
			using (var dbContext = _databaseContextFactory.CreateDatabaseContext())
			{
				await dbContext.RegisterMigrationAsync(migrationPath, migrationHash, DateTime.UtcNow);
			}
		}
	}
}
