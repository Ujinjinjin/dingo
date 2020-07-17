using Dingo.Core.Abstractions;
using Dingo.Core.Config;
using Dingo.Core.Extensions;
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
		private readonly IRenderer _renderer;

		public MigrationOperations(
			IConfigWrapper configWrapper,
			IDatabaseHelper databaseHelper,
			IDirectoryScanner directoryScanner,
			IHashMaker hashMaker,
			IPathHelper pathHelper,
			IRenderer renderer
		)
		{
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
			_databaseHelper = databaseHelper ?? throw new ArgumentNullException(nameof(databaseHelper));
			_directoryScanner = directoryScanner ?? throw new ArgumentNullException(nameof(directoryScanner));
			_hashMaker = hashMaker ?? throw new ArgumentNullException(nameof(hashMaker));
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
			_renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
		}

		/// <inheritdoc />
		public async Task HandshakeDatabaseConnectionAsync(string configPath = null)
		{
			await _configWrapper.LoadAsync(configPath);

			if (await _databaseHelper.HandshakeDatabaseConnectionAsync())
			{
				await _renderer.ShowMessageAsync("Successfully connected to database, dingo is ready to go!", MessageType.Info);
			}
			else
			{
				await _renderer.ShowMessageAsync("Connection to database cannot be established. Please, check your configs and try again.", MessageType.Error);
			}
		}

		/// <inheritdoc />
		public async Task RunMigrationsAsync(
			string migrationsRootPath,
			string configPath = null,
			bool silent = false,
			string connectionString = null,
			string providerName = null,
			string migrationSchema = null,
			string migrationTable = null
		)
		{
			await _configWrapper.LoadAsync(configPath);

			_configWrapper.ConnectionString = connectionString ?? _configWrapper.ConnectionString;
			_configWrapper.ProviderName = providerName ?? _configWrapper.ProviderName;
			_configWrapper.MigrationSchema = migrationSchema ?? _configWrapper.MigrationSchema;
			_configWrapper.MigrationTable = migrationTable ?? _configWrapper.MigrationTable;

			if (!await _databaseHelper.HandshakeDatabaseConnectionAsync())
			{
				await _renderer.ShowMessageAsync("Unable to run migrations, because connection to database cannot be established. Please, check your configs and try again.", MessageType.Error);
				return;
			}

			await RunSystemMigrationsAsync(silent);
			
			await _renderer.PrintBreakLineAsync(silent);
			
			await RunProjectMigrationsAsync(migrationsRootPath, silent);
		}

		/// <inheritdoc />
		public async Task ShowMigrationsStatusAsync(string migrationsRootPath, string configPath = null, bool silent = false)
		{
			await _configWrapper.LoadAsync(configPath);

			if (!await _databaseHelper.HandshakeDatabaseConnectionAsync())
			{
				await _renderer.ShowMessageAsync("Unable to show migrations status, because connection to database cannot be established. Please, check your configs and try again.", MessageType.Error);
				return;
			}

			await RunSystemMigrationsAsync(true);

			var filePathList = await _directoryScanner.GetFilePathListAsync(migrationsRootPath, _configWrapper.MigrationsSearchPattern);
			var migrationInfoList = await _hashMaker.GetMigrationInfoListAsync(filePathList);
			var migrationsStatusList = await _databaseHelper.GetMigrationsStatusAsync(migrationInfoList);

			await _renderer.ShowMigrationsStatusAsync(migrationsStatusList, silent);
		}

		/// <summary> Read all migrations from specified path and apply if needed </summary>
		/// <param name="migrationsRootPath">Root path where all project migrations are stored</param>
		/// <param name="silent">Show less info about migration status</param>
		private async Task RunProjectMigrationsAsync(string migrationsRootPath, bool silent)
		{
			await _renderer.PrintTextAsync("Running project migrations...", silent);

			var filePathList = await _directoryScanner.GetFilePathListAsync(migrationsRootPath, _configWrapper.MigrationsSearchPattern);
			var migrationInfoList = await _hashMaker.GetMigrationInfoListAsync(filePathList);

			await ReadAndApplyMigrationList(migrationInfoList, silent);
		}

		/// <summary> Read all system migrations and apply if needed </summary>
		private async Task RunSystemMigrationsAsync(bool silent)
		{
			await _renderer.PrintTextAsync("Running system migrations...", silent);

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
				await ReadAndApplyMigrationList(migrationInfoList, silent);
			}
		}

		/// <summary> Read migration files and apply if needed </summary>
		/// <param name="migrationInfoList">List of migration infos</param>
		/// <param name="silent">Show less info on progress</param>
		private async Task ReadAndApplyMigrationList(IList<MigrationInfo> migrationInfoList, bool silent)
		{
			var migrationsStatusList = await _databaseHelper.GetMigrationsStatusAsync(migrationInfoList);
			await _renderer.ShowMigrationsStatusAsync(migrationsStatusList, silent);
			
			for (var i = 0; i < migrationsStatusList.Count; i++)
			{
				await _renderer.PrintTextAsync($"{i + 1}) Processing migration '{migrationsStatusList[i].Path.Relative}' - {migrationsStatusList[i].Status.ToDisplayText()}", silent);

				if (migrationsStatusList[i].Status == MigrationStatus.UpToDate)
				{
					await _renderer.PrintTextAsync("Skipping action.", silent);
					continue;
				}

				await _renderer.PrintTextAsync("Reading migration file contents...", silent);
				var sqlScriptText = await File.ReadAllTextAsync(migrationInfoList[i].Path.Absolute);

				await _renderer.PrintTextAsync("Applying migration...", silent);
				await _databaseHelper.ApplyMigrationAsync(sqlScriptText, migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash);

				await _renderer.PrintTextAsync("Migration successfully applied.", silent);
			}
		}
	}
}