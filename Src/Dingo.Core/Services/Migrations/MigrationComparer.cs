using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Dingo.Core.Repository.Models;
using Trico.Configuration;

namespace Dingo.Core.Services.Migrations;

internal sealed class MigrationComparer : IMigrationComparer
{
	private readonly IRepository _repository;
	private readonly IConfiguration _configuration;

	public MigrationComparer(IRepository repository, IConfiguration configuration)
	{
		_repository = repository.Required(nameof(repository));
		_configuration = configuration.Required(nameof(configuration));
	}

	public async Task<IReadOnlyList<Migration>> CalculateMigrationsStatusAsync(
		IReadOnlyList<Migration> migrations,
		CancellationToken ct = default
	)
	{
		if (!await IsDatabaseAvailableAsync())
		{
			throw new ConnectionNotEstablishedException();
		}

		if (await _repository.IsDatabaseEmptyAsync(ct))
		{
			return AllMigrationsAreNew(migrations);
		}

		var migrationsComparison = (await _repository.GetMigrationsComparisonAsync(migrations, ct))
			.GroupBy(x => x.MigrationHash)
			.ToDictionary(x => x.Key, x => x.Single());
		migrations = CalculateMigrationsStatus(migrations, migrationsComparison);

		return migrations;
	}

	private async Task<bool> IsDatabaseAvailableAsync()
	{
		return await _repository.TryHandshakeAsync();
	}

	private IReadOnlyList<Migration> AllMigrationsAreNew(IReadOnlyList<Migration> migrations)
	{
		foreach (var migration in migrations)
		{
			migration.Status = MigrationStatus.New;
		}

		return migrations;
	}

	private IReadOnlyList<Migration> CalculateMigrationsStatus(
		IReadOnlyList<Migration> migrations,
		IDictionary<string, MigrationComparisonOutput> migrationComparison
	)
	{
		if (migrations.Count != migrationComparison.Count)
		{
			throw new MigrationMismatchException("count", "Can't calculate migration statuses. Error on db side");
		}

		var forceDirs = GetForcePaths();

		foreach (var migration in migrations)
		{
			if (!migrationComparison.TryGetValue(migration.Hash.Value, out var comparisonOutput))
			{
				throw new MigrationMismatchException("hash", "Can't calculate migration statuses. Migration path mismatch");
			}

			if (IsMigrationInForcePath(forceDirs, migration))
			{
				migration.Status = MigrationStatus.ForceOutdated;
				continue;
			}

			migration.Status = comparisonOutput.HashMatches switch
			{
				null => MigrationStatus.New,
				true => MigrationStatus.UpToDate,
				false => MigrationStatus.Outdated,
			};
		}

		return migrations;
	}

	private IReadOnlyList<string> GetForcePaths()
	{
		var paths = _configuration.Get(Configuration.Key.MigrationForcePaths);
		if (string.IsNullOrWhiteSpace(paths))
		{
			return Array.Empty<string>();
		}

		return paths.Split(Constants.PathArraySeparator);
	}

	private bool IsMigrationInForcePath(IReadOnlyList<string> forcePaths, Migration migration)
	{
		return forcePaths.Any(fd => migration.Path.Relative.StartsWith(fd));
	}
}
