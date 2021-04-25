using Dingo.Core.Abstractions;
using Dingo.Core.Adapters;
using Dingo.Core.Config;
using Dingo.Core.Extensions;
using Dingo.Core.Helpers;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Dingo.Core.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	/// <inheritdoc />
	internal class MigrationOperations : IMigrationOperations
	{
		private readonly IConfigWrapper _configWrapper;
		private readonly IDatabaseRepository _databaseRepository;
		private readonly IDirectoryAdapter _directoryAdapter;
		private readonly IDirectoryScanner _directoryScanner;
		private readonly IFileAdapter _fileAdapter;
		private readonly IHashMaker _hashMaker;
		private readonly IPathHelper _pathHelper;
		private readonly IRenderer _renderer;
		private readonly ILogger _logger;

		public MigrationOperations(
			IConfigWrapper configWrapper,
			IDatabaseRepository databaseRepository,
			IDirectoryAdapter directoryAdapter,
			IDirectoryScanner directoryScanner,
			IFileAdapter fileAdapter,
			IHashMaker hashMaker,
			IPathHelper pathHelper,
			IRenderer renderer,
			ILoggerFactory loggerFactory
		)
		{
			_configWrapper = configWrapper ?? throw new ArgumentNullException(nameof(configWrapper));
			_databaseRepository = databaseRepository ?? throw new ArgumentNullException(nameof(databaseRepository));
			_directoryAdapter = directoryAdapter ?? throw new ArgumentNullException(nameof(directoryAdapter));
			_directoryScanner = directoryScanner ?? throw new ArgumentNullException(nameof(directoryScanner));
			_fileAdapter = fileAdapter ?? throw new ArgumentNullException(nameof(fileAdapter));
			_hashMaker = hashMaker ?? throw new ArgumentNullException(nameof(hashMaker));
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
			_renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
			_logger = loggerFactory?.CreateLogger<MigrationOperations>() ?? throw new ArgumentNullException(nameof(loggerFactory));
		}

		/// <inheritdoc />
		public async Task CreateMigrationFileAsync(string name, string path)
		{
			using var _ = new CodeTiming(_logger);

			try
			{
				if (!_directoryAdapter.Exists(path))
				{
					_directoryAdapter.CreateDirectory(path);
				}

				_fileAdapter
					.Create(path.ConcatPath($"{DateTime.UtcNow:yyyyMMddHHmmss}_{name}.sql"))
					?.Close();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "MigrationOperations:CreateMigrationFileAsync:Error;");
				await _renderer.ShowMessageAsync("Error occured while creating migration file", MessageType.Error);
			}
		}

		/// <inheritdoc />
		public async Task HandshakeDatabaseConnectionAsync(string configPath = null)
		{
			using var _ = new CodeTiming(_logger);

			try
			{
				await _configWrapper.LoadAsync(configPath);

				if (await _databaseRepository.HandshakeDatabaseConnectionAsync())
				{
					await _renderer.ShowMessageAsync("Successfully connected to database, dingo is ready to go!", MessageType.Info);
				}
				else
				{
					await _renderer.ShowMessageAsync("Connection to database cannot be established. Please, check your configs and try again.", MessageType.Error);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "MigrationOperations:HandshakeDatabaseConnectionAsync:Error;");
				await _renderer.ShowMessageAsync(ex.Message, MessageType.Error);
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
			using var _ = new CodeTiming(_logger);

			try
			{
				await _configWrapper.LoadAsync(configPath);

				_configWrapper.ConnectionString = connectionString ?? _configWrapper.ConnectionString;
				_configWrapper.ProviderName = providerName ?? _configWrapper.ProviderName;
				_configWrapper.MigrationSchema = migrationSchema ?? _configWrapper.MigrationSchema;
				_configWrapper.MigrationTable = migrationTable ?? _configWrapper.MigrationTable;

				if (!await _databaseRepository.HandshakeDatabaseConnectionAsync())
				{
					await _renderer.ShowMessageAsync("Unable to run migrations, because connection to database cannot be established. Please, check your configs and try again.", MessageType.Error);
					return;
				}

				await RunSystemMigrationsAsync(silent);

				await _renderer.PrintBreakLineAsync(silent, newLineBefore: false, newLineAfter: false);

				await RunProjectMigrationsAsync(migrationsRootPath, silent);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "MigrationOperations:RunMigrationsAsync:Error;");
				await _renderer.ShowMessageAsync(ex.Message, MessageType.Error);
			}
		}

		/// <inheritdoc />
		public async Task ShowMigrationsStatusAsync(string migrationsRootPath, string configPath = null, bool silent = false)
		{
			using var _ = new CodeTiming(_logger);

			try
			{
				await _configWrapper.LoadAsync(configPath);

				if (!await _databaseRepository.HandshakeDatabaseConnectionAsync())
				{
					await _renderer.ShowMessageAsync("Unable to show migrations status, because connection to database cannot be established. Please, check your configs and try again.", MessageType.Error);
					return;
				}

				await RunSystemMigrationsAsync(true);

				var filePathList = _directoryScanner.GetFilePathList(migrationsRootPath, _configWrapper.MigrationsSearchPattern);
				var migrationInfoList = _hashMaker.GetMigrationInfoList(filePathList);
				var migrationsStatusList = await _databaseRepository.GetMigrationsStatusAsync(migrationInfoList);

				await _renderer.ShowMigrationsStatusAsync(migrationsStatusList, silent);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "MigrationOperations:ShowMigrationsStatusAsync:Error;");
				await _renderer.ShowMessageAsync(ex.Message, MessageType.Error);
			}
		}

		/// <summary> Read all migrations from specified path and apply if needed </summary>
		/// <param name="migrationsRootPath">Root path where all project migrations are stored</param>
		/// <param name="silent">Show less info about migration status</param>
		private async Task RunProjectMigrationsAsync(string migrationsRootPath, bool silent)
		{
			using var _ = new CodeTiming(_logger);

			await _renderer.PrintTextAsync("Running project migrations...", silent);

			var filePathList = _directoryScanner.GetFilePathList(migrationsRootPath, _configWrapper.MigrationsSearchPattern);
			var migrationInfoList = _hashMaker.GetMigrationInfoList(filePathList);

			await ReadAndApplyMigrationList(migrationInfoList, silent, true);
		}

		/// <summary> Read all system migrations and apply if needed </summary>
		private async Task RunSystemMigrationsAsync(bool silent)
		{
			using var _ = new CodeTiming(_logger);

			await _renderer.PrintTextAsync("Running system migrations...", silent);

			try
			{
				await _databaseRepository.InstallCheckTableExistenceProcedureAsync();
			}
			catch (Exception exception)
			{
				await _renderer.PrintTextAsync($"Error applying migration: {_configWrapper.CheckTableExistenceProcedurePath}. ERROR: {exception.Message}", false);
				throw;
			}

			var migrationTableExists = await _databaseRepository.CheckMigrationTableExistenceAsync();

			var migrationsRootPath = _pathHelper.GetApplicationBaseDirectory() + _configWrapper.DingoMigrationsRootPath;
			var filePathList = _directoryScanner.GetFilePathList(migrationsRootPath, _configWrapper.MigrationsSearchPattern);

			var migrationInfoList = _hashMaker.GetMigrationInfoList(filePathList);

			if (!migrationTableExists)
			{
				for (var i = 0; i < migrationInfoList.Count; i++)
				{
					var sqlScriptText = await _fileAdapter.ReadAllTextAsync(migrationInfoList[i].Path.Absolute);
					await TryApplyMigrationAsync(sqlScriptText, migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash, false);
				}

				for (var i = 0; i < migrationInfoList.Count; i++)
				{
					await _databaseRepository.RegisterMigrationAsync(migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash);
				}
			}
			else
			{
				await ReadAndApplyMigrationList(migrationInfoList, silent, false);
			}
		}

		/// <summary> Read migration files and apply if needed </summary>
		/// <param name="migrationInfoList">List of migration infos</param>
		/// <param name="silent">Show less info on progress</param>
		/// <param name="isProject">Describes if project migrations are being applied</param>
		private async Task ReadAndApplyMigrationList(IList<MigrationInfo> migrationInfoList, bool silent, bool isProject)
		{
			using var _ = new CodeTiming(_logger);

			var migrationsStatusList = await _databaseRepository.GetMigrationsStatusAsync(migrationInfoList);
			if (isProject)
			{
				await _renderer.ShowMigrationsStatusAsync(migrationsStatusList, silent);	
			}

			migrationsStatusList = migrationsStatusList
				.Where(x => x.Status != MigrationStatus.UpToDate)
				.ToArray();

			if (migrationsStatusList.Count == 0)
			{
				await _renderer.PrintTextAsync("Everything is up to date, no actions required.", silent);
				return;
			}
			for (var i = 0; i < migrationsStatusList.Count; i++)
			{
				await _renderer.PrintTextAsync($"{i + 1}) Processing '{migrationsStatusList[i].Path.Relative}'", silent);
				await _renderer.PrintTextAsync($"Status: {migrationsStatusList[i].Status.ToDisplayText()}", silent);

				await _renderer.PrintTextAsync("Reading migration file contents...", silent);
				var sqlScriptText = await _fileAdapter.ReadAllTextAsync(migrationInfoList[i].Path.Absolute);

				await _renderer.PrintTextAsync("Applying migration...", silent);
				await TryApplyMigrationAsync(sqlScriptText, migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash, true);

				await _renderer.PrintTextAsync("Migration successfully applied.", silent);
			}
		}

		private async Task TryApplyMigrationAsync(string sql, string migrationPath, string migrationHash, bool registerMigrations)
		{
			try
			{
				await _databaseRepository.ApplyMigrationAsync(sql, migrationPath, migrationHash, registerMigrations);
			}
			catch (Exception exception)
			{
				await _renderer.PrintTextAsync($"Error applying migration: {migrationPath}. ERROR: {exception.Message}", false);
				throw;
			}
		}
	}
}