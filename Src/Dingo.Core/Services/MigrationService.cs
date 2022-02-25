using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dingo.Core.Abstractions;
using Dingo.Core.Adapters;
using Dingo.Core.Config;
using Dingo.Core.Extensions;
using Dingo.Core.Helpers;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Dingo.Core.Utils;
using Dingo.Core.Validators;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Services;

/// <inheritdoc />
internal sealed class MigrationService : IMigrationService
{
	private readonly IConfigWrapper _configWrapper;
	private readonly IDatabaseRepository _databaseRepository;
	private readonly IDirectoryAdapter _directoryAdapter;
	private readonly IDirectoryScanner _directoryScanner;
	private readonly IFileAdapter _fileAdapter;
	private readonly IHashMaker _hashMaker;
	private readonly IPathHelper _pathHelper;
	private readonly IRenderer _renderer;
	private readonly IValidator<string> _migrationNameValidator;
	private readonly ILogger _logger;

	public MigrationService(
		IConfigWrapper configWrapper,
		IDatabaseRepository databaseRepository,
		IDirectoryAdapter directoryAdapter,
		IDirectoryScanner directoryScanner,
		IFileAdapter fileAdapter,
		IHashMaker hashMaker,
		IPathHelper pathHelper,
		IRenderer renderer,
		MigrationNameValidator migrationNameValidator,
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
		_migrationNameValidator = migrationNameValidator ?? throw new ArgumentNullException(nameof(migrationNameValidator));
		_logger = loggerFactory?.CreateLogger<MigrationService>() ?? throw new ArgumentNullException(nameof(loggerFactory));
	}

	/// <inheritdoc />
	public async Task CreateMigrationFileAsync(string name, string path)
	{
		using var _ = new CodeTiming(_logger);

		try
		{
			if (!_migrationNameValidator.Validate(name))
			{
				await _renderer.ShowMessageAsync("Invalid migration name. Filename must contain only latin symbols, numbers and underscore", MessageType.Warning);
				return;
			}
				
			if (!_directoryAdapter.Exists(path))
			{
				_directoryAdapter.CreateDirectory(path);
			}

			var dateTimeString = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
			_fileAdapter
				.Create(path.ConcatPath($"{dateTimeString}_{name}.sql"))
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
		string migrationsDir,
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
			await _renderer.PrintTextAsync("Running project migrations...", silent, TextStyle.Info);

			var filePathList = _directoryScanner.GetFilePathList(migrationsDir, _configWrapper.MigrationsSearchPattern);

			if (await AnyMigrationHasInvalidFilenameAsync(filePathList))
			{
				return;
			}
				
			var migrationInfoList = await _hashMaker.GetMigrationInfoListAsync(filePathList);

			await ReadAndApplyMigrationList(migrationInfoList, silent, true);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "MigrationOperations:RunMigrationsAsync:Error;");
			await _renderer.ShowMessageAsync(ex.Message, MessageType.Error);
		}
	}

	/// <inheritdoc />
	public async Task ShowMigrationsStatusAsync(string migrationsDir, string configPath = null, bool silent = false)
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

			var filePathList = _directoryScanner.GetFilePathList(migrationsDir, _configWrapper.MigrationsSearchPattern);

			if (await AnyMigrationHasInvalidFilenameAsync(filePathList))
			{
				return;
			}

			var migrationInfoList = await _hashMaker.GetMigrationInfoListAsync(filePathList);
			var migrationsStatusList = await _databaseRepository.GetMigrationsStatusAsync(migrationInfoList);

			await _renderer.ShowMigrationsStatusAsync(migrationsStatusList, silent);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "MigrationOperations:ShowMigrationsStatusAsync:Error;");
			await _renderer.ShowMessageAsync(ex.Message, MessageType.Error);
		}
	}

	/// <summary> Read all system migrations and apply if needed </summary>
	private async Task RunSystemMigrationsAsync(bool silent)
	{
		using var _ = new CodeTiming(_logger);

		await _renderer.PrintTextAsync("Running system migrations...", silent, TextStyle.Info);

		try
		{
			await _databaseRepository.InstallCheckTableExistenceProcedureAsync();
		}
		catch (Exception exception)
		{
			await _renderer.ShowMessageAsync($"Migration: {_configWrapper.TableExistsProcedurePath}. {exception.Message}", MessageType.Error);
			throw;
		}

		var migrationTableExists = await _databaseRepository.CheckMigrationTableExistenceAsync();

		var migrationsDir = _pathHelper.GetApplicationBaseDirectory() + _configWrapper.DingoMigrationsDir;
		var filePathList = _directoryScanner.GetFilePathList(migrationsDir, _configWrapper.MigrationsSearchPattern);

		var migrationInfoList = await _hashMaker.GetMigrationInfoListAsync(filePathList);

		if (!migrationTableExists)
		{
			for (var i = 0; i < migrationInfoList.Count; i++)
			{
				var sqlScriptText = await _fileAdapter.ReadAllTextAsync(migrationInfoList[i].Path.Absolute);

				try
				{
					await _databaseRepository.ApplyMigrationAsync(sqlScriptText, migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash, false);
				}
				catch (Exception exception)
				{
					await _renderer.ShowMessageAsync($"Migration: {migrationInfoList[i].Path.Relative}. {exception.Message}", MessageType.Error);
					throw;
				}
			}

			for (var i = 0; i < migrationInfoList.Count; i++)
			{
				await _databaseRepository.RegisterMigrationAsync(migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash);
			}

			await _databaseRepository.ReloadDatabaseTypesAsync();
		}
		else
		{
			await ReadAndApplyMigrationList(migrationInfoList, true, false);
		}
	}

	/// <summary> Read migration files and apply if needed </summary>
	/// <param name="migrationInfoList">List of migration infos</param>
	/// <param name="silent">Show less info on progress</param>
	/// <param name="isProject">Describes if project migrations are being applied</param>
	private async Task ReadAndApplyMigrationList(IList<MigrationInfo> migrationInfoList, bool silent, bool isProject)
	{
		using var _ = new CodeTiming(_logger);

		migrationInfoList = await _databaseRepository.GetMigrationsStatusAsync(migrationInfoList);
		if (isProject)
		{
			await _renderer.ShowMigrationsStatusAsync(migrationInfoList, silent);	
		}

		var migrationCount = 0;
		for (var i = 0; i < migrationInfoList.Count; i++)
		{
			if (migrationInfoList[i].Status == MigrationStatus.UpToDate)
			{
				continue;
			}
				
			await _renderer.PrintTextAsync($"{(++migrationCount).ToString()}) Processing '{migrationInfoList[i].Path.Relative}'", silent);
			await _renderer.PrintTextAsync($"\tStatus: {migrationInfoList[i].Status.ToDisplayText()}", silent);

			await _renderer.PrintTextAsync("\tReading migration file contents...", silent);
			var sqlScriptText = await _fileAdapter.ReadAllTextAsync(migrationInfoList[i].Path.Absolute);

			await _renderer.PrintTextAsync("\tApplying migration...", silent);

			try
			{
				await _databaseRepository.ApplyMigrationAsync(sqlScriptText, migrationInfoList[i].Path.Relative, migrationInfoList[i].NewHash);
			}
			catch (Exception exception)
			{
				await _renderer.ShowMessageAsync($"Migration: {migrationInfoList[i].Path.Relative}. {exception.Message}", MessageType.Error);
				throw;
			}

			await _renderer.PrintTextAsync("\tMigration successfully applied.", silent);
		}

		if (isProject && migrationCount > 0)
		{
			await _renderer.ShowMessageAsync($"{migrationCount.ToString()} migrations were successfully applied", MessageType.Success);
		}
	}

	/// <summary> Validate given filename list and show warning if it contains invalid ones </summary>
	/// <param name="filePathList">List of file paths to validate</param>
	/// <returns>True, if given list contains invalid filenames; False otherwise</returns>
	private async Task<bool> AnyMigrationHasInvalidFilenameAsync(IList<FilePath> filePathList)
	{
		var invalidMigrationFilenames = new List<string>();

		for (var i = 0; i < filePathList.Count; i++)
		{
			if (filePathList[i].IsValid)
			{
				continue;
			}
				
			invalidMigrationFilenames.Add(filePathList[i].Relative);
		}

		if (invalidMigrationFilenames.Count == 0)
		{
			return false;
		}

		await _renderer.ShowMessageAsync("Operation aborted!", MessageType.Warning);

		for (var i = 0; i < invalidMigrationFilenames.Count; i++)
		{
			await _renderer.PrintTextAsync($"Invalid migration filename: {invalidMigrationFilenames[i]}", textStyle: TextStyle.Warning);
		}
			
		await _renderer.PrintTextAsync("Filename must contain only latin symbols, numbers and underscore", textStyle: TextStyle.Warning);

		return true;
	}
}