using Dingo.Core.Config;
using Dingo.Core.Helpers;
using Dingo.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	/// <inheritdoc />
	internal class MigrationOperations : IMigrationOperations
	{
		private readonly IConfigWrapper _configWrapper;
		private readonly IDatabaseHelper _databaseHelper;
		private readonly IDirectoryScanner _directoryScanner;
		private readonly IHashMaker _hashMaker;
		private readonly IPathHelper _pathHelper;

		public MigrationOperations(
			IConfigWrapper configWrapper,
			IDatabaseHelper databaseHelper,
			IDirectoryScanner directoryScanner,
			IHashMaker hashMaker,
			IPathHelper pathHelper
		)
		{
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
			_databaseHelper = databaseHelper ?? throw new ArgumentNullException(nameof(databaseHelper));
			_directoryScanner = directoryScanner ?? throw new ArgumentNullException(nameof(directoryScanner));
			_hashMaker = hashMaker ?? throw new ArgumentNullException(nameof(hashMaker));
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		}
		
		/// <inheritdoc />
		public async Task RunMigrationsAsync(string projectMigrationsRootPath)
		{
			await RunSystemMigrationsAsync();
			await RunProjectMigrationsAsync(projectMigrationsRootPath);
		}

		/// <summary> Read all migrations from specified path and apply if needed </summary>
		/// <param name="migrationsRootPath">Root path where all project migrations are stored</param>
		private async Task RunProjectMigrationsAsync(string migrationsRootPath)
		{
			var filePathList = await _directoryScanner.GetFilePathListAsync(migrationsRootPath, _configWrapper.MigrationsSearchPattern);
			var migrationInfoList = await _hashMaker.GetMigrationInfoListAsync(filePathList);
			
			await ReadAndApplyMigrationList(migrationInfoList);
		}

		/// <summary> Read all system migrations and apply if needed </summary>
		private async Task RunSystemMigrationsAsync()
		{
			await _databaseHelper.InstallCheckTableExistenceProcedureAsync();

			var migrationTableExists = await _databaseHelper.CheckMigrationTableExistenceAsync();

			var migrationsRootPath = _pathHelper.GetApplicationBaseDirectory() + _configWrapper.DingoMigrationsRootPath;
			var filePathList = await _directoryScanner.GetFilePathListAsync(migrationsRootPath, _configWrapper.MigrationsSearchPattern);

			var migrationInfoList = await _hashMaker.GetMigrationInfoListAsync(filePathList);

			if (!migrationTableExists)
			{
				for (var i = 0; i < migrationInfoList.Count; i++)
				{
					var sqlScriptText = await File.ReadAllTextAsync(migrationInfoList[i].Path.Absolute);
					await _databaseHelper.ApplyMigrationAsync(sqlScriptText, migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash, true);
				}
				
				for (var i = 0; i < migrationInfoList.Count; i++)
				{
					await _databaseHelper.RegisterMigrationAsync(migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash);
				}
			}
			else
			{
				await ReadAndApplyMigrationList(migrationInfoList);
			}
		}

		/// <summary> Read migration files and apply if needed </summary>
		/// <param name="migrationInfoList">List of migration infos</param>
		private async Task ReadAndApplyMigrationList(IList<MigrationInfo> migrationInfoList)
		{
			var migrationsStatusList = await _databaseHelper.GetMigrationsStatusAsync(migrationInfoList);
			for (var i = 0; i < migrationsStatusList.Count; i++)
			{
				if (migrationsStatusList[i].Action == MigrationAction.Skip)
					continue;
					
				var sqlScriptText = await File.ReadAllTextAsync(migrationInfoList[i].Path.Absolute);
				await _databaseHelper.ApplyMigrationAsync(sqlScriptText, migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash);
			}
		}
	}
}