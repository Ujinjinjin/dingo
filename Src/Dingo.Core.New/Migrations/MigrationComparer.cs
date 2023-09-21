using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Dingo.Core.Repository.Models;
using Trico.Configuration;

namespace Dingo.Core.Migrations;

public class MigrationComparer : IMigrationComparer
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
		if (!IsDatabaseAvailable())
		{
			throw new ConnectionNotEstablishedException();
		}

		if (await IsDatabaseEmptyAsync(ct))
		{
			return AllMigrationsAreNew(migrations);
		}

		var migrationsComparison = await _repository.GetMigrationsComparisonAsync(migrations, ct);
		migrations = CalculateMigrationsStatus(migrations, migrationsComparison);

		return migrations;
	}

	private bool IsDatabaseAvailable()
	{
		return _repository.TryHandshake();
	}

	private async Task<bool> IsDatabaseEmptyAsync(CancellationToken ct = default)
	{
		var dingoSchemaName = _configuration.Get(Configuration.Key.SchemaName);
		return !await _repository.SchemaExistsAsync(dingoSchemaName, ct);
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
		IReadOnlyList<MigrationComparisonOutput> migrationComparison
	)
	{
		if (migrations.Count != migrationComparison.Count)
		{
			throw new MigrationMismatchException("count", "Can't calculate migration statuses. Error on db side");
		}

		for (var i = 0; i < migrations.Count; i++)
		{
			if (!IsSameMigration(migrations[i], migrationComparison[i]))
			{
				throw new MigrationMismatchException("hash", "Can't calculate migration statuses. Migration path mismatch");
			}

			migrations[i].Status = migrationComparison[i].HashMatches switch
			{
				null => MigrationStatus.New,
				true => MigrationStatus.UpToDate,
				false => MigrationStatus.Outdated,
			};
		}

		return migrations;
	}

	private bool IsSameMigration(Migration migration, MigrationComparisonOutput migrationComparison)
	{
		return migration.Hash.Value == migrationComparison.Hash;
	}
}
