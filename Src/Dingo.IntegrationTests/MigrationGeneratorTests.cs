using AutoFixture;
using Dingo.Core.Services.Migrations;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.IntegrationTests;

public class MigrationGeneratorTests : IntegrationTestBase
{
	[Fact]
	public async Task MigrationGeneratorTests_GenerateAsync__WhenValidNameAndPathGiven_ThenFileCreated()
	{
		// arrange
		var generator = ServiceProvider.GetService<IMigrationGenerator>();
		var timestamp = DateTime.UtcNow.Ticks;
		var filename = $"{nameof(MigrationGeneratorTests)}{timestamp}";
		var path = "./temp/migrations";

		// act & assert
		generator.Should().NotBeNull();
		await generator.GenerateAsync(filename, path);

		// migration must be created
		var migrationFiles = Directory.GetFiles(path);
		migrationFiles.Should().NotBeEmpty();
		migrationFiles.Should().ContainSingle(x => x.Contains(filename));

		// migration must be empty
		var migrationFilePath = migrationFiles.Single(x => x.Contains(filename));
		var migrationContents = await File.ReadAllTextAsync(migrationFilePath);
		migrationContents.Should().BeEmpty();
	}

	[Fact]
	public async Task MigrationGeneratorTests_GenerateAsync__WhenNotExistingPathGiven_ThenDirectoryAndFileCreated()
	{
		// arrange
		var generator = ServiceProvider.GetService<IMigrationGenerator>();
		var timestamp = DateTime.UtcNow.Ticks;
		var filename = $"{nameof(MigrationGeneratorTests)}{timestamp}";
		var directory = Fixture.Create<string>();
		var path = $"./temp/migrations/{directory}";

		// act & assert
		generator.Should().NotBeNull();
		await generator.GenerateAsync(filename, path);
		var dirExists = Directory.Exists(path);

		// directory must exist
		dirExists.Should().BeTrue();

		// migration must be created
		var migrationFiles = Directory.GetFiles(path);
		migrationFiles.Should().NotBeEmpty();
		migrationFiles.Should().ContainSingle(x => x.Contains(filename));
	}
}
