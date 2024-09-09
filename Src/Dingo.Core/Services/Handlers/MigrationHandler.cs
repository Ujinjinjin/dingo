using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;
using Dingo.Core.IO;
using Dingo.Core.Services.Config;
using Dingo.Core.Services.Migrations;
using Dingo.Core.Utils;
using Microsoft.Extensions.Logging;
using Trico.Configuration;

namespace Dingo.Core.Services.Handlers;

internal sealed class MigrationHandler : IMigrationHandler
{
	private const int DefaultPatchRollbackCount = 1;

	private readonly IMigrationGenerator _migrationGenerator;
	private readonly IMigrationComparer _migrationComparer;
	private readonly IMigrationScanner _migrationScanner;
	private readonly IMigrationRunner _migrationRunner;
	private readonly IConfigProfileLoader _profileLoader;
	private readonly IConfiguration _configuration;
	private readonly IOutput _output;
	private readonly ILogger _logger;

	public MigrationHandler(
		IMigrationGenerator migrationGenerator,
		IMigrationComparer migrationComparer,
		IMigrationScanner migrationScanner,
		IMigrationRunner migrationRunner,
		IConfigProfileLoader profileLoader,
		IConfiguration configuration,
		IOutput output,
		ILoggerFactory loggerFactory
	)
	{
		_migrationGenerator = migrationGenerator.Required(nameof(migrationGenerator));
		_migrationComparer = migrationComparer.Required(nameof(migrationComparer));
		_migrationScanner = migrationScanner.Required(nameof(migrationScanner));
		_migrationRunner = migrationRunner.Required(nameof(migrationRunner));
		_profileLoader = profileLoader.Required(nameof(profileLoader));
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
			_logger.LogError(ex, "MigrationHandler:CreateAsync:Error");
			_output.Write("Error occured while creating migration file", LogLevel.Error);
		}
	}

	/// <inheritdoc />
	public async Task MigrateAsync(string? profile, string? path, CancellationToken ct = default)
	{
		using var _ = new CodeTiming(_logger);

		try
		{
			await _profileLoader.LoadAsync(profile, ct);
			await _migrationRunner.MigrateAsync(ResolvePath(path), ct);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "MigrationHandler:MigrateAsync:Error");
			_output.Write($"Error occured while applying migrations: {ex.Message}", LogLevel.Error);
		}
	}

	/// <inheritdoc />
	public async Task RollbackAsync(
		string? profile,
		string? path,
		int? patchCount,
		bool force,
		CancellationToken ct = default
	)
	{
		using var _ = new CodeTiming(_logger);

		try
		{
			await _profileLoader.LoadAsync(profile, ct);
			await _migrationRunner.RollbackAsync(ResolvePath(path), patchCount ?? DefaultPatchRollbackCount, force, ct);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "MigrationHandler:RollbackAsync:Error");
			_output.Write($"Error occured while rolling back migrations: {ex.Message}", LogLevel.Error);
		}
	}

	/// <inheritdoc />
	public async Task ShowStatusAsync(string? profile, string? path, CancellationToken ct = default)
	{
		using var _ = new CodeTiming(_logger);

		try
		{
			await _profileLoader.LoadAsync(profile, ct);
			await _ShowStatusAsync(ResolvePath(path), ct);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "MigrationHandler:ShowStatusAsync:Error");
			_output.Write("Error occured while retrieving migrations status", LogLevel.Error);
		}
	}

	private async Task _ShowStatusAsync(string path, CancellationToken ct = default)
	{
		var migrations = await _migrationScanner.ScanAsync(path, ct);
		await _migrationComparer.CalculateMigrationsStatusAsync(migrations, ct);

		_output.Write($"Total count: {migrations.Count}", LogLevel.Information);
		foreach (var migration in migrations)
		{
			_output.Write($"{migration.Status} - '{migration.Path.Relative}'", LogLevel.Information);
		}
	}

	private string ResolvePath(string? path)
	{
		return path ?? _configuration.Get(Configuration.Key.MigrationPath) ?? throw new PathNotProvidedException();
	}
}
