using Dingo.Core.Extensions;
using Dingo.Core.IO;
using Dingo.Core.Migrations;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Dingo.Core.Utils;
using Microsoft.Extensions.Logging;
using Trico.Configuration;

namespace Dingo.Core.Handlers;

internal class MigrationHandler : IMigrationHandler
{
	private readonly IMigrationApplier _migrationApplier;
	private readonly IMigrationGenerator _migrationGenerator;
	private readonly IMigrationComparer _migrationComparer;
	private readonly IMigrationScanner _migrationScanner;
	private readonly IMigrationPathBuilder _migrationPathBuilder;
	private readonly IConfiguration _configuration;
	private readonly IRepository _repository;
	private readonly IOutput _output;
	private readonly ILogger _logger;

	public MigrationHandler(
		IMigrationApplier migrationApplier,
		IMigrationGenerator migrationGenerator,
		IMigrationComparer migrationComparer,
		IMigrationScanner migrationScanner,
		IMigrationPathBuilder migrationPathBuilder,
		IConfiguration configuration,
		IRepository repository,
		IOutput output,
		ILoggerFactory loggerFactory
	)
	{
		_migrationApplier = migrationApplier.Required(nameof(migrationApplier));
		_migrationGenerator = migrationGenerator.Required(nameof(migrationGenerator));
		_migrationComparer = migrationComparer.Required(nameof(migrationComparer));
		_migrationScanner = migrationScanner.Required(nameof(migrationScanner));
		_migrationPathBuilder = migrationPathBuilder.Required(nameof(migrationPathBuilder));
		_repository = repository.Required(nameof(repository));
		_configuration = configuration.Required(nameof(configuration));
		_output = output.Required(nameof(output));
		_logger = loggerFactory.Required(nameof(loggerFactory))
			.CreateLogger<MigrationHandler>()
			.Required(nameof(loggerFactory));
	}

	/// <inheritdoc />
	public async Task CreateAsync(string name, string path, CancellationToken ct = default)
	{
		using var _ = new CodeTiming(_logger);

		try
		{
			await _migrationGenerator.GenerateAsync(name, path, ct);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "MigrationService:CreateAsync:Error;");
			_output.Write("Error occured while creating migration file", LogLevel.Error);
		}
	}

	public async Task MigrateAsync(string path, CancellationToken ct = default)
	{
		using var _ = new CodeTiming(_logger);

		try
		{
			await _MigrateAsync(path, ct);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "MigrationService:MigrateAsync:Error;");
			_output.Write($"Error occured while applying migrations: {ex.Message}", LogLevel.Error);
		}
	}

	private async Task _MigrateAsync(string path, CancellationToken ct = default)
	{
		await ApplySystemMigrationsAsync(ct);
		var migrations = await _migrationScanner.ScanAsync(path, ct);
		await ApplyMigrationsAsync(migrations, MigrationType.User, ct);
	}

	private async Task ApplySystemMigrationsAsync(CancellationToken ct = default)
	{
		var systemMigrationsPath = _migrationPathBuilder.BuildSystemMigrationsPath();
		var migrations = await _migrationScanner.ScanAsync(systemMigrationsPath, ct);

		if (await IsDatabaseEmptyAsync(ct))
		{
			await InitializeDatabaseAsync(migrations, ct);
			await _repository.ReloadTypesAsync(ct);
		}

		await ApplyMigrationsAsync(migrations, MigrationType.System, ct);
	}

	private async Task InitializeDatabaseAsync(IReadOnlyList<Migration> migrations, CancellationToken ct = default)
	{
		foreach (var migration in migrations)
		{
			await _migrationApplier.ApplyAsync(migration, ct);
		}

		var patch = await _repository.GetNextPatchAsync(ct);

		foreach (var migration in migrations)
		{
			await _migrationApplier.RegisterAsync(migration, patch, ct);
		}
	}

	private async Task ApplyMigrationsAsync(
		IReadOnlyList<Migration> migrations,
		MigrationType migrationType,
		CancellationToken ct = default
	)
	{
		var migrationsToApply = (await _migrationComparer.CalculateMigrationsStatusAsync(migrations, ct))
			.Where(x => x.Status is MigrationStatus.New or MigrationStatus.Outdated)
			.ToArray();

		if (migrationsToApply.Length == 0)
		{
			_output.Write($"All {migrationType} migrations are up to date, skipping", LogLevel.Information);
			return;
		}

		var patchNumber = await _repository.GetNextPatchAsync(ct);
		var total = migrationsToApply.Length;
		var current = 1;
		_output.Write($"Patch {patchNumber}; Migrations to apply: {total}", LogLevel.Information);

		foreach (var migration in migrationsToApply)
		{
			_output.Write($"{current++}/{total} Applying '{migration.Path.Relative}'", LogLevel.Information);
			await _migrationApplier.ApplyAndRegisterAsync(migration, patchNumber, ct);
		}

		_output.Write($"Finished applying {migrationType} migrations.", LogLevel.Information);
	}

	public async Task RollbackAsync()
	{
	}

	public async Task ShowStatusAsync(string path, CancellationToken ct = default)
	{
		using var _ = new CodeTiming(_logger);

		try
		{
			await _ShowStatusAsync(path, ct);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "MigrationService:ShowStatusAsync:Error;");
			_output.Write("Error occured while retrieving migrations status", LogLevel.Error);
		}
	}

	private async Task _ShowStatusAsync(string path, CancellationToken ct = default)
	{
		var migrations = await _migrationScanner.ScanAsync(path, ct);
		await _migrationComparer.CalculateMigrationsStatusAsync(migrations, ct);

		_output.Write($"Total count: {migrations.Count}", LogLevel.Information);
		for (var i = 0; i < migrations.Count; i++)
		{
			_output.Write($"{migrations[i].Status} - '{migrations[i].Path.Relative}'", LogLevel.Information);
		}
	}

	private async Task<bool> IsDatabaseEmptyAsync(CancellationToken ct = default)
	{
		var dingoSchemaName = _configuration.Get(Configuration.Key.SchemaName);
		return !await _repository.SchemaExistsAsync(dingoSchemaName, ct);
	}
}
