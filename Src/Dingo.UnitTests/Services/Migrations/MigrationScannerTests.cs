using Dingo.Core;
using Dingo.Core.Models;
using Dingo.Core.Services.Adapters;
using Dingo.Core.Services.Helpers;
using Dingo.Core.Services.Migrations;
using Trico.Configuration;

namespace Dingo.UnitTests.Services.Migrations;

public class MigrationScannerTests : UnitTestBase
{
	[Fact]
	public async Task MigrationScannerTests_ScanAsync__WhenGivenDirHasNoMigrations_ThenEmptyCollectionReturned()
	{
		// arrange
		const int migrationCount = 0;
		var configuration = SetupConfiguration();
		var commandParser = SetupCommandParser();
		var directoryScanner = SetupDirectoryScanner(migrationCount);
		var fileAdapter = SetupFileAdapter();
		var migrationScanner = new MigrationScanner(
			configuration,
			commandParser,
			directoryScanner,
			fileAdapter
		);

		// act
		var migrations = await migrationScanner.ScanAsync(Fixture.Create<string>());

		// assert
		migrations.Should().NotBeNull();
		migrations.Count.Should().Be(migrationCount);
	}

	[Fact]
	public async Task MigrationScannerTests_ScanAsync__WhenGivenDirHasMigrations_ThenExactNumberOfMigrationsReturned()
	{
		// arrange
		var migrationCount = Fixture.Create<int>();
		var configuration = SetupConfiguration();
		var commandParser = SetupCommandParser();
		var directoryScanner = SetupDirectoryScanner(migrationCount);
		var fileAdapter = SetupFileAdapter();
		var migrationScanner = new MigrationScanner(
			configuration,
			commandParser,
			directoryScanner,
			fileAdapter
		);

		// act
		var migrations = await migrationScanner.ScanAsync(Fixture.Create<string>());

		// assert
		migrations.Should().NotBeNull();
		migrations.Count.Should().Be(migrationCount);
	}

	[Fact]
	public async Task MigrationScannerTests_ScanAsync__WhenMigrationsScanned_ThenAllMigrationsMustHaveUnknownStatus()
	{
		// arrange
		var migrationCount = Fixture.Create<int>();
		var configuration = SetupConfiguration();
		var commandParser = SetupCommandParser();
		var directoryScanner = SetupDirectoryScanner(migrationCount);
		var fileAdapter = SetupFileAdapter();
		var migrationScanner = new MigrationScanner(
			configuration,
			commandParser,
			directoryScanner,
			fileAdapter
		);

		// act
		var migrations = await migrationScanner.ScanAsync(Fixture.Create<string>());

		// assert
		migrations.Should().NotBeNull();
		migrations.Should().AllSatisfy(migration => migration.Status.Should().Be(MigrationStatus.None));
	}

	private IConfiguration SetupConfiguration()
	{
		var config = new Mock<IConfiguration>();
		config.Setup(c => c.Get(It.Is<string>(name => name == Configuration.Key.MigrationWildcard)))
			.Returns(Fixture.Create<string>());

		return config.Object;
	}

	private IMigrationCommandParser SetupCommandParser()
	{
		var parser = new Mock<IMigrationCommandParser>();

		return parser.Object;
	}

	private IDirectoryScanner SetupDirectoryScanner(int migrationsCount)
	{
		var paths = GetMockMigrationPaths(migrationsCount);
		return SetupDirectoryScanner(paths);
	}

	private MigrationPath[] GetMockMigrationPaths(int count)
	{
		var paths = new MigrationPath[count];
		for (var i = 0; i < count; i++)
		{
			paths[i] = Fixture.Create<MigrationPath>();
		}

		return paths;
	}

	private IDirectoryScanner SetupDirectoryScanner(MigrationPath[] paths)
	{
		var scanner = new Mock<IDirectoryScanner>();

		scanner.Setup(s => s.Scan(It.IsAny<string>(), It.IsAny<string>()))
			.Returns(paths);

		return scanner.Object;
	}

	private IFile SetupFileAdapter()
	{
		var adapter = new Mock<IFile>();

		adapter.Setup(a => a.ReadAllTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Fixture.Create<string>());

		return adapter.Object;
	}
}
