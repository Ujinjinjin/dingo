using AutoFixture;
using Dingo.Core;
using Dingo.Core.Services.Config;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Dingo.IntegrationTests.Services;

public class ConfigGeneratorTests : IntegrationTestBase
{
	[Fact]
	public void ConfigGeneratorTests_Generate__WhenNotExistingPathGiven_ThenDirectoryAndConfigProfileCreated()
	{
		// arrange
		var generator = ServiceProvider.GetService<IConfigGenerator>();
		var directory = Fixture.Create<string>();
		var path = $"./temp/config/{directory}";
		var dingoDir = $"{path}/{Constants.ConfigDir}";

		// act
		generator.Generate(path);

		// assert
		Directory.Exists(dingoDir).Should().BeTrue();
		var configFiles = Directory.GetFiles(dingoDir);
		configFiles.Should().NotBeEmpty();
		configFiles.Should().ContainSingle(x => x.Contains("config.yml"));
	}

	[Fact]
	public void ConfigGeneratorTests_Generate__WhenProfileNotGiven_ThenDefaultConfigProfileCreated()
	{
		// arrange
		var generator = ServiceProvider.GetService<IConfigGenerator>();
		var directory = Fixture.Create<string>();
		var path = $"./temp/config/{directory}";
		var dingoDir = $"{path}/{Constants.ConfigDir}";

		// act
		generator.Generate(path);

		// assert
		var configFiles = Directory.GetFiles(dingoDir);
		configFiles.Should().NotBeEmpty();
		configFiles.Should().ContainSingle(x => x.Contains("config.yml"));
	}

	[Fact]
	public void ConfigGeneratorTests_Generate__WhenProfileGiven_ThenConfigProfileCreated()
	{
		// arrange
		var generator = ServiceProvider.GetService<IConfigGenerator>();
		var directory = Fixture.Create<string>();
		var profile = Fixture.Create<string>();
		var path = $"./temp/config/{directory}";
		var dingoDir = $"{path}/{Constants.ConfigDir}";
		var configFileName = $"config.{profile}.yml";

		// act
		generator.Generate(path, profile);

		// assert
		var configFiles = Directory.GetFiles(dingoDir);
		configFiles.Should().NotBeEmpty();
		configFiles.Should().ContainSingle(x => x.Contains(configFileName));
	}
}
