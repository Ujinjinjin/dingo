using System.Text;
using Dingo.Core.Extensions;
using Dingo.Core.IO;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Dingo.Core.Repository.UoW;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Services.Migrations;

internal sealed class MigrationRunner : IMigrationRunner
{
	private readonly IMigrationApplier _migrationApplier;
	private readonly IMigrationComparer _migrationComparer;
	private readonly IMigrationScanner _migrationScanner;
	private readonly IMigrationPathBuilder _migrationPathBuilder;
	private readonly IMigrationStatusCalculator _migrationStatusCalculator;
	private readonly IUnitOfWorkFactory _unitOfWorkFactory;
	private readonly IRepository _repository;
	private readonly IOutput _output;

	public MigrationRunner(
		IMigrationApplier migrationApplier,
		IMigrationComparer migrationComparer,
		IMigrationScanner migrationScanner,
		IMigrationPathBuilder migrationPathBuilder,
		IUnitOfWorkFactory unitOfWorkFactory,
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
		_unitOfWorkFactory = unitOfWorkFactory.Required(nameof(unitOfWorkFactory));
		_repository = repository.Required(nameof(repository));
		_output = output.Required(nameof(output));
	}

	public async Task MigrateAsync(string path, CancellationToken ct = default)
	{
		await ApplySystemMigrationsAsync(ct);
		var migrations = await _migrationScanner.ScanAsync(path, ct);
		await ApplyMigrationsAsync(migrations, PatchType.User, LogLevel.Information, ct);
	}

	private async Task ApplySystemMigrationsAsync(CancellationToken ct = default)
	{
		var systemMigrationsPath = _migrationPathBuilder.BuildSystemMigrationsPath();
		var migrations = await _migrationScanner.ScanAsync(systemMigrationsPath, ct);

		if (await _repository.IsDatabaseEmptyAsync(ct))
		{
			await RunInTransaction(
				async () => await ApplySystemMigrationsOnEmptyDatabaseAsync(migrations, ct),
				LogLevel.None,
				ct
			);
		}
		else
		{
			await ApplyMigrationsAsync(migrations, PatchType.System, LogLevel.None, ct);
		}
	}

	private async Task ApplySystemMigrationsOnEmptyDatabaseAsync(
		IReadOnlyList<Migration> migrations,
		CancellationToken ct = default
	)
	{
		foreach (var migration in migrations)
		{
			await _migrationApplier.ApplyAsync(migration, ct);
		}

		var patch = await _repository.GetNextPatchAsync(PatchType.System, ct);

		foreach (var migration in migrations)
		{
			await _migrationApplier.RegisterAsync(migration, patch, ct);
		}

		await _repository.CompletePatchAsync(patch, ct);
		await _repository.ReloadTypesAsync(ct);
	}

	private async Task ApplyMigrationsAsync(
		IReadOnlyList<Migration> migrations,
		PatchType patchType,
		LogLevel logLevel,
		CancellationToken ct = default
	)
	{
		var migrationsToApply = (await _migrationComparer.CalculateMigrationsStatusAsync(migrations, ct))
			.Where(_migrationStatusCalculator.NeedToApplyMigration)
			.ToArray();

		if (migrationsToApply.Length == 0)
		{
			_output.Write($"All {patchType} migrations are up to date, skipping", LogLevel.Information);
			return;
		}

		await RunInTransaction(
			async () => {
				var patch = await _repository.GetNextPatchAsync(patchType, ct);
				var total = migrationsToApply.Length;
				var current = 1;
				_output.Write($"Patch #{patch}; {patchType} migrations to apply: {total}", LogLevel.Information);

				foreach (var migration in migrationsToApply)
				{
					_output.Write($"{current++}/{total} Applying '{migration.Path.Relative}'", logLevel);
					await _migrationApplier.ApplyAndRegisterAsync(migration, patch, ct);
				}

				await _repository.CompletePatchAsync(patch, ct);
			},
			logLevel,
			ct
		);

		_output.Write($"Finished applying {patchType} migrations.", LogLevel.Information);
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

			var reverseOrderedPaths = migrations
				.Select(x => x.MigrationPath)
				.OrderMigrations()
				.Reverse()
				.ToArray();

			await RunInTransaction(
				async () => {
					var current = 1;
					foreach (var migrationPath in reverseOrderedPaths)
					{
						_output.Write($"{current++}/{totalMigrations} Reverting '{migrationPath.Relative}'", LogLevel.Information);
						var localMigration = localMigrationsMap[migrationPath.Relative];
						await _migrationApplier.RevertAsync(localMigration, ct);
					}

					await _repository.RevertPatchAsync(patch, ct);
					rolledBackCount++;
				},
				LogLevel.Information,
				ct
			);
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
				sb.Append($"Can't find local migration file {migration.MigrationPath.Relative}. ");
				sb.Append("Most probably it was deleted from the directory.");
				sb.Append(Environment.NewLine);
				canRollback = false;
			}

			if (status.HasFlag(PatchMigrationStatus.LocalMigrationModified))
			{
				sb.Append($"Local migration {migration.MigrationPath.Relative} was modified since last patch, ");
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

	private async Task RunInTransaction(Func<Task> action, LogLevel logLevel, CancellationToken ct = default)
	{
		await using var unitOfWork = await _unitOfWorkFactory.CreateAsync();
		try
		{
			_output.Write($"Transaction start. ID: {unitOfWork.Id.ToString()}", logLevel);
			await unitOfWork.BeginAsync(ct);
			await action();
			await unitOfWork.CommitAsync(ct);
			_output.Write($"Transaction commited. ID: {unitOfWork.Id.ToString()}", logLevel);
		}
		catch (Exception)
		{
			await unitOfWork.RollbackAsync(ct);
			_output.Write($"Transaction rolled back. ID: {unitOfWork.Id.ToString()}", logLevel);
			throw;
		}
	}
}
