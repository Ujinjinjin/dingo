using Dingo.Core.Exceptions;
using Dingo.Core.Migrations;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Dingo.Core.Repository.Models;
using Trico.Configuration;

namespace Dingo.UnitTests.Migrations;

public class MigrationComparerTests : UnitTestBase
{
	[Fact]
	public async Task
		MigrationComparerTests_CalculateMigrationsStatusAsync__WhenDatabaseNotAvailable_ThenExceptionThrown()
	{
		// arrange
		var initialMigrations = CreateMigrations(Fixture.Create<string>());
		var repository = SetupRepository(databaseAvailable: false);
		var configuration = SetupConfiguration();
		var comparer = new MigrationComparer(repository, configuration);

		// act
		var func = async () => await comparer.CalculateMigrationsStatusAsync(initialMigrations);

		// assert
		await func.Should().ThrowAsync<ConnectionNotEstablishedException>();
	}

	[Fact]
	public async Task
		MigrationComparerTests_CalculateMigrationsStatusAsync__WhenDatabaseIsEmpty_ThenAllMigrationsAreNew()
	{
		// arrange
		var initialMigrations = CreateMigrations(Fixture.Create<string>());
		var repository = SetupRepository(schemaExists: false);
		var configuration = SetupConfiguration();
		var comparer = new MigrationComparer(repository, configuration);

		// act
		var updatedMigrations = await comparer.CalculateMigrationsStatusAsync(initialMigrations);

		// assert
		updatedMigrations.Should().HaveSameCount(initialMigrations);
		updatedMigrations.Should().AllSatisfy(x => x.Status.Should().Be(MigrationStatus.New));
	}

	[Fact]
	public async Task
		MigrationComparerTests_CalculateMigrationsStatusAsync__WhenGivenMigrationsCountNotMatchesDbComparisonCount_ThenExceptionThrown()
	{
		// arrange
		var count = Fixture.Create<int>();
		var migrationPath = Fixture.Create<string>();
		var initialMigrations = CreateMigrations(migrationPath, count);
		var migrationComparison = CreateMigrationComparisonOutput(false, migrationPath, count + 1);

		var repository = SetupRepository(migrationComparison: migrationComparison);
		var configuration = SetupConfiguration();
		var comparer = new MigrationComparer(repository, configuration);

		// act
		var func = async () => await comparer.CalculateMigrationsStatusAsync(initialMigrations);

		// assert
		await func.Should().ThrowAsync<MigrationMismatchException>().Where(x => x.ParamName == "count");
	}

	[Fact]
	public async Task
		MigrationComparerTests_CalculateMigrationsStatusAsync__WhenGivenMigrationsHashNotMatchesDbComparisonHash_ThenExceptionThrown()
	{
		// arrange
		var count = Fixture.Create<int>();
		var initialMigrations = CreateMigrations(Fixture.Create<string>(), count);
		var migrationComparison = CreateMigrationComparisonOutput(false, Fixture.Create<string>(), count);

		var repository = SetupRepository(migrationComparison: migrationComparison);
		var configuration = SetupConfiguration();
		var comparer = new MigrationComparer(repository, configuration);

		// act
		var func = async () => await comparer.CalculateMigrationsStatusAsync(initialMigrations);

		// assert
		await func.Should().ThrowAsync<MigrationMismatchException>().Where(x => x.ParamName == "hash");
	}

	[Fact]
	public async Task
		MigrationComparerTests_CalculateMigrationsStatusAsync__WhenGivenMigrationNotInDb_ThenMigrationStatusIsNew()
	{
		// arrange
		var count = 1;
		var migrationPath = Fixture.Create<string>();
		var initialMigrations = CreateMigrations(migrationPath, count);
		var migrationComparison = CreateMigrationComparisonOutput(null, migrationPath, count);

		var repository = SetupRepository(migrationComparison: migrationComparison);
		var configuration = SetupConfiguration();
		var comparer = new MigrationComparer(repository, configuration);

		// act
		var updatedMigrations = await comparer.CalculateMigrationsStatusAsync(initialMigrations);

		// assert
		updatedMigrations.Should().NotBeEmpty();
		updatedMigrations.Should().HaveCount(count);
		updatedMigrations.Should().AllSatisfy(x => x.Status.Should().Be(MigrationStatus.New));
	}

	[Fact]
	public async Task
		MigrationComparerTests_CalculateMigrationsStatusAsync__WhenHashMatches_ThenMigrationStatusIsUpToDate()
	{
		// arrange
		var count = 1;
		var migrationPath = Fixture.Create<string>();
		var initialMigrations = CreateMigrations(migrationPath, count);
		var migrationComparison = CreateMigrationComparisonOutput(true, migrationPath, count);

		var repository = SetupRepository(migrationComparison: migrationComparison);
		var configuration = SetupConfiguration();
		var comparer = new MigrationComparer(repository, configuration);

		// act
		var updatedMigrations = await comparer.CalculateMigrationsStatusAsync(initialMigrations);

		// assert
		updatedMigrations.Should().NotBeEmpty();
		updatedMigrations.Should().HaveCount(count);
		updatedMigrations.Should().AllSatisfy(x => x.Status.Should().Be(MigrationStatus.UpToDate));
	}

	[Fact]
	public async Task
		MigrationComparerTests_CalculateMigrationsStatusAsync__WhenHashNotMatches_ThenMigrationStatusIsOutdated()
	{
		// arrange
		var count = 1;
		var migrationPath = Fixture.Create<string>();
		var initialMigrations = CreateMigrations(migrationPath, count);
		var migrationComparison = CreateMigrationComparisonOutput(false, migrationPath, count);

		var repository = SetupRepository(migrationComparison: migrationComparison);
		var configuration = SetupConfiguration();
		var comparer = new MigrationComparer(repository, configuration);

		// act
		var updatedMigrations = await comparer.CalculateMigrationsStatusAsync(initialMigrations);

		// assert
		updatedMigrations.Should().NotBeEmpty();
		updatedMigrations.Should().HaveCount(count);
		updatedMigrations.Should().AllSatisfy(x => x.Status.Should().Be(MigrationStatus.Outdated));
	}

	private IRepository SetupRepository(
		bool databaseAvailable = true,
		bool schemaExists = true,
		IReadOnlyList<MigrationComparisonOutput>? migrationComparison = null
	)
	{
		var repository = new Mock<IRepository>();

		repository.Setup(r => r.TryHandshake())
			.Returns(databaseAvailable);

		repository.Setup(r => r.SchemaExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(schemaExists);

		migrationComparison ??= Fixture.CreateMany<MigrationComparisonOutput>().ToArray();
		repository.Setup(
				r => r.GetMigrationsComparisonAsync(It.IsAny<IReadOnlyList<Migration>>(), It.IsAny<CancellationToken>())
			)
			.ReturnsAsync(migrationComparison);

		return repository.Object;
	}

	private IConfiguration SetupConfiguration()
	{
		var config = new Mock<IConfiguration>();

		return config.Object;
	}

	private IReadOnlyList<Migration> CreateMigrations(
		string hash,
		int? count = null
	)
	{
		count ??= 3;
		Fixture.Register(() => new Hash(hash));
		Fixture.Register(
			() => new Migration(
				Fixture.Create<MigrationPath>(),
				Fixture.Create<Hash>(),
				Fixture.Create<MigrationCommand>()
			)
		);

		return Fixture.CreateMany<Migration>(count.Value).ToArray();
	}

	private IReadOnlyList<MigrationComparisonOutput> CreateMigrationComparisonOutput(
		bool? hashMatches,
		string hash,
		int? count = null
	)
	{
		count ??= 3;
		Fixture.Register(() => new MigrationComparisonOutput(hash, hashMatches));

		return Fixture.CreateMany<MigrationComparisonOutput>(count.Value).ToArray();
	}
}
