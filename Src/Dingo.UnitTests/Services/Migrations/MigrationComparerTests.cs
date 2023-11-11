using Dingo.Core;
using Dingo.Core.Exceptions;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Dingo.Core.Repository.Models;
using Dingo.Core.Services.Migrations;
using Trico.Configuration;

namespace Dingo.UnitTests.Services.Migrations;

public class MigrationComparerTests : UnitTestBase
{
	[Fact]
	public async Task MigrationComparerTests_CalculateMigrationsStatusAsync__WhenDatabaseNotAvailable_ThenExceptionThrown()
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
	public async Task MigrationComparerTests_CalculateMigrationsStatusAsync__WhenDatabaseIsEmpty_ThenAllMigrationsAreNew()
	{
		// arrange
		var initialMigrations = CreateMigrations(Fixture.Create<string>());
		var repository = SetupRepository(databaseIsEmpty: true);
		var configuration = SetupConfiguration();
		var comparer = new MigrationComparer(repository, configuration);

		// act
		var updatedMigrations = await comparer.CalculateMigrationsStatusAsync(initialMigrations);

		// assert
		updatedMigrations.Should().HaveSameCount(initialMigrations);
		updatedMigrations.Should().AllSatisfy(x => x.Status.Should().Be(MigrationStatus.New));
	}

	[Fact]
	public async Task MigrationComparerTests_CalculateMigrationsStatusAsync__WhenGivenMigrationsCountNotMatchesDbComparisonCount_ThenExceptionThrown()
	{
		// arrange
		var count = 1;
		var migrationHash = Fixture.Create<string>();
		var initialMigrations = CreateMigrations(migrationHash, count);
		var migrationComparison = CreateMigrationComparisonOutput(false, null, count + 1);

		var repository = SetupRepository(migrationComparison: migrationComparison);
		var configuration = SetupConfiguration();
		var comparer = new MigrationComparer(repository, configuration);

		// act
		var func = async () => await comparer.CalculateMigrationsStatusAsync(initialMigrations);

		// assert
		await func.Should().ThrowAsync<MigrationMismatchException>().Where(x => x.ParamName == "count");
	}

	[Fact]
	public async Task MigrationComparerTests_CalculateMigrationsStatusAsync__WhenGivenMigrationsHashNotMatchesDbComparisonHash_ThenExceptionThrown()
	{
		// arrange
		var count = 1;
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
	public async Task MigrationComparerTests_CalculateMigrationsStatusAsync__WhenGivenMigrationNotInDb_ThenMigrationStatusIsNew()
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
	public async Task MigrationComparerTests_CalculateMigrationsStatusAsync__WhenHashMatches_ThenMigrationStatusIsUpToDate()
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
	public async Task MigrationComparerTests_CalculateMigrationsStatusAsync__WhenHashNotMatches_ThenMigrationStatusIsOutdated()
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



	[Theory]
	[InlineData(null)]
	[InlineData(false)]
	[InlineData(true)]
	public async Task MigrationComparerTests_CalculateMigrationsStatusAsync__WhenMigrationIsInForceDir_ThenMigrationStatusIsForce(bool? hashMatches)
	{
		// arrange
		var count = 1;
		var migrationPath = Fixture.Create<string>();
		var initialMigrations = CreateMigrations(migrationPath, count);
		var migrationComparison = CreateMigrationComparisonOutput(hashMatches, migrationPath, count);
		var migration = initialMigrations[0];

		var repository = SetupRepository(migrationComparison: migrationComparison);
		var configuration = SetupConfiguration(migration.Path.Relative);
		var comparer = new MigrationComparer(repository, configuration);

		// act
		var updatedMigrations = await comparer.CalculateMigrationsStatusAsync(initialMigrations);

		// assert
		updatedMigrations.Should().NotBeEmpty();
		updatedMigrations.Should().HaveCount(count);
		updatedMigrations.Should().AllSatisfy(x => x.Status.Should().Be(MigrationStatus.ForceOutdated));
	}

	private IRepository SetupRepository(
		bool databaseAvailable = true,
		bool databaseIsEmpty = false,
		IReadOnlyList<MigrationComparisonOutput>? migrationComparison = null
	)
	{
		var repository = new Mock<IRepository>();

		repository.Setup(r => r.TryHandshakeAsync(default))
			.ReturnsAsync(databaseAvailable);

		repository.Setup(r => r.IsDatabaseEmptyAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(databaseIsEmpty);

		migrationComparison ??= Fixture.CreateMany<MigrationComparisonOutput>().ToArray();
		repository.Setup(r => r.GetMigrationsComparisonAsync(It.IsAny<IReadOnlyList<Migration>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(migrationComparison);

		return repository.Object;
	}

	private IConfiguration SetupConfiguration(string? forceDir = default)
	{
		var config = new Mock<IConfiguration>();

		config.Setup(x => x.Get(It.Is<string>(s => s == Configuration.Key.MigrationForcePaths)))
			.Returns(forceDir ?? string.Empty);

		return config.Object;
	}
}
