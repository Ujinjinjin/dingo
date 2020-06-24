using Dingo.Core.Config;
using Dingo.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	internal class MigrationOperations : IMigrationOperations
	{
		private readonly IConfigWrapper _configWrapper;
		private readonly IDatabaseOperations _databaseOperations;
		private readonly IDirectoryScanner _directoryScanner;
		private readonly IHashMaker _hashMaker;
		private readonly IPathHelper _pathHelper;

		public MigrationOperations(
			IConfigWrapper configWrapper,
			IDatabaseOperations databaseOperations,
			IDirectoryScanner directoryScanner,
			IHashMaker hashMaker,
			IPathHelper pathHelper
		)
		{
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
			_databaseOperations = databaseOperations ?? throw new ArgumentNullException(nameof(databaseOperations));
			_directoryScanner = directoryScanner ?? throw new ArgumentNullException(nameof(directoryScanner));
			_hashMaker = hashMaker ?? throw new ArgumentNullException(nameof(hashMaker));
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		}

		/// <summary> Run migrations </summary>
		/// <param name="projectMigrationsRootPath"> Root path where all project migrations are stored </param>
		public async Task RunMigrationsAsync(string projectMigrationsRootPath)
		{
			await RunSystemMigrationsAsync();
			await RunProjectMigrationsAsync(projectMigrationsRootPath);
		}

		/// <summary> Read all migrations from specified path and apply if needed </summary>
		/// <param name="migrationsRootPath"> Root path where all project migrations are stored </param>
		private async Task RunProjectMigrationsAsync(string migrationsRootPath)
		{
			var filePathList = await _directoryScanner.GetFilePathListAsync(migrationsRootPath, _configWrapper.MigrationsSearchPattern);
			var migrationInfoList = await _hashMaker.GetMigrationInfoListAsync(filePathList);
			
			await ReadAndApplyMigrationList(migrationInfoList);
		}

		/// <summary> Read all system migrations and apply if needed </summary>
		private async Task RunSystemMigrationsAsync()
		{
			await _databaseOperations.InstallCheckTableExistenceProcedureAsync();

			var migrationTableExists = await _databaseOperations.CheckMigrationTableExistenceAsync();

			var migrationsRootPath = _pathHelper.GetApplicationBaseDirectory() + _configWrapper.DingoMigrationsRootPath;
			var filePathList = await _directoryScanner.GetFilePathListAsync(migrationsRootPath, _configWrapper.MigrationsSearchPattern);

			var migrationInfoList = await _hashMaker.GetMigrationInfoListAsync(filePathList);

			if (!migrationTableExists)
			{
				for (var i = 0; i < migrationInfoList.Count; i++)
				{
					var sqlScriptText = await File.ReadAllTextAsync(migrationInfoList[i].Path.Absolute);
					await _databaseOperations.ApplyMigrationAsync(sqlScriptText, migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash, true);
				}
				
				for (var i = 0; i < migrationInfoList.Count; i++)
				{
					await _databaseOperations.RegisterMigrationAsync(migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash);
				}
			}
			else
			{
				await ReadAndApplyMigrationList(migrationInfoList);
			}
		}

		/// <summary> Read migration files and apply if needed </summary>
		/// <param name="migrationInfoList"> List of migration infos </param>
		private async Task ReadAndApplyMigrationList(IList<MigrationInfo> migrationInfoList)
		{
			var migrationsStatusList = await _databaseOperations.GetMigrationsStatusAsync(migrationInfoList);
			for (var i = 0; i < migrationsStatusList.Count; i++)
			{
				if (migrationsStatusList[i].Action == MigrationAction.Skip)
					continue;
					
				var sqlScriptText = await File.ReadAllTextAsync(migrationInfoList[i].Path.Absolute);
				await _databaseOperations.ApplyMigrationAsync(sqlScriptText, migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash);
			}
		}
	}
}