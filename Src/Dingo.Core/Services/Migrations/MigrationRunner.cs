using System.Text;
using Dingo.Core.Extensions;
using Dingo.Core.IO;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Dingo.Core.Repository.Models;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Services.Migrations;

internal class MigrationRunner : IMigrationRunner
{
	private readonly IMigrationApplier _migrationApplier;
	private readonly IMigrationComparer _migrationComparer;
	private readonly IMigrationScanner _migrationScanner;
	private readonly IMigrationPathBuilder _migrationPathBuilder;
	private readonly IMigrationStatusCalculator _migrationStatusCalculator;
	private readonly IRepository _repository;
	private readonly IOutput _output;

	public MigrationRunner(
		IMigrationApplier migrationApplier,
		IMigrationComparer migrationComparer,
		IMigrationScanner migrationScanner,
		IMigrationPathBuilder migrationPathBuilder,
		IRepository repository,
		IOutput output,
		IMigrationStatusCalculator migrationStatusCalculator
	)
	{
		_migrationApplier = migrationApplier.Required(nameof(migrationApplier));
		_migrationComparer = migrationComparer.Required(nameof(migrationComparer));
		_migrationScanner = migrationScanner.Required(nameof(migrationScanner));
		_migrationPathBuilder = migrationPathBuilder.Required(nameof(migrationPathBuilder));
		_migrationStatusCalculator = migrationStatusCalculator.Required(nameof(migrationStatusCalculator));
		_repository = repository.Required(nameof(repository));
		_output = output.Required(nameof(output));
	}

	public async Task MigrateAsync(string path, CancellationToken ct = default)
	{
		await ApplySystemMigrationsAsync(ct);
		var migrations = await _migrationScanner.ScanAsync(path, ct);
		await ApplyMigrationsAsync(migrations, MigrationType.User, ct);
	}

	private async Task ApplySystemMigrationsAsync(CancellationToken ct = default)
	{
		var systemMigrationsPath = _migrationPathBuilder.BuildSystemMigrationsPath();
		var migrations = await _migrationScanner.ScanAsync(systemMigrationsPath, ct);

		if (await _repository.IsDatabaseEmptyAsync(ct))
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

		await _repository.CompletePatchAsync(patch, ct);
	}

	private async Task ApplyMigrationsAsync(
		IReadOnlyList<Migration> migrations,
		MigrationType migrationType,
		CancellationToken ct = default
	)
	{
		var migrationsToApply = (await _migrationComparer.CalculateMigrationsStatusAsync(migrations, ct))
			.Where(_migrationStatusCalculator.NeedToApplyMigration)
			.ToArray();

		if (migrationsToApply.Length == 0)
		{
			_output.Write($"All {migrationType} migrations are up to date, skipping", LogLevel.Information);
			return;
		}

		var patch = await _repository.GetNextPatchAsync(ct);
		var total = migrationsToApply.Length;
		var current = 1;
		_output.Write($"Patch #{patch}; Migrations to apply: {total}", LogLevel.Information);

		foreach (var migration in migrationsToApply)
		{
			_output.Write($"{current++}/{total} Applying '{migration.Path.Relative}'", LogLevel.Information);
			await _migrationApplier.ApplyAndRegisterAsync(migration, patch, ct);
		}

		await _repository.CompletePatchAsync(patch, ct);

		_output.Write($"Finished applying {migrationType} migrations.", LogLevel.Information);
	}

	public async Task RollbackAsync(string path, int patchCount, bool force, CancellationToken ct = default)
	{
		var patchMigrations = (await _repository.GetLastPatchMigrationsAsync(patchCount, ct))
			.GroupBy(x => x.PatchNumber)
			.OrderByDescending(x => x.Key)
			.ToDictionary(x => x.Key, x => x.ToArray());

		var localMigrations = await _migrationComparer.CalculateMigrationsStatusAsync(
			await _migrationScanner.ScanAsync(path, ct),
			ct
		);
		var localMigrationsMap = localMigrations
			.GroupBy(x => x.Path.Relative)
			.ToDictionary(x => x.Key, x => x.Single());

		_output.Write($"Patches to rollback: {patchMigrations.Count}", LogLevel.Information);
		var rolledBackCount = 0;
		foreach (var (patch, migrations) in patchMigrations)
		{
			var totalMigrations = migrations.Length;
			_output.Write($"Rolling back patch #{patch}; Total migrations: {totalMigrations}", LogLevel.Information);

			if (!CanRollbackPatch(migrations, localMigrationsMap, force))
			{
				_output.Write($"Patch #{patch} can't be rolled back, terminating operation", LogLevel.Warning);
				break;
			}

			foreach (var migration in migrations)
			{
				var localMigration = localMigrationsMap[migration.MigrationPath];
				await _migrationApplier.RevertAsync(localMigration, ct);
			}

			await _repository.RevertPatchAsync(patch, ct);
			rolledBackCount++;
		}

		_output.Write($"Finished. Rolled back {rolledBackCount} patches.", LogLevel.Information);
	}

	private bool CanRollbackPatch(
		IReadOnlyList<PatchMigration> migrations,
		Dictionary<string, Migration> localMigrationsMap,
		bool force
	)
	{
		var sb = new StringBuilder();
		var canRollback = true;
		foreach (var migration in migrations)
		{
			var status = _migrationStatusCalculator.CalculatePatchMigrationStatus(migration, localMigrationsMap);

			if (status.HasFlag(PatchMigrationStatus.LocalMigrationNotFound))
			{
				sb.Append($"Can't find local migration file {migration.MigrationPath}. ");
				sb.Append("Most probably it was deleted from the directory.");
				sb.Append(Environment.NewLine);
				canRollback = false;
			}

			if (status.HasFlag(PatchMigrationStatus.LocalMigrationModified))
			{
				sb.Append($"Local migration {migration.MigrationPath} was modified since last patch, ");
				sb.Append("rolling it back might cause unexpected behaviour.");
				sb.Append(Environment.NewLine);
				canRollback = false;
			}
		}

		canRollback |= force;
		if (!canRollback)
		{
			sb.Append(Environment.NewLine);
			sb.Append("To ignore the issues above execute this command with `--force` flag");
		}

		_output.Write(sb.ToString(), LogLevel.Warning);

		return canRollback;
	}
}
