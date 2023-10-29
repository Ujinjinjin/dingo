using Dingo.Core.Services.Adapters;
using Dingo.Core.Services.Config;

namespace Dingo.UnitTests.Services.Config;

public class ConfigGeneratorTests : UnitTestBase
{
	[Fact]
	public void ConfigGeneratorTests_Generate__SmokeTest()
	{
		// arrange
		var file = SetupFileAdapter();
		var path = SetupPathAdapter();
		var directory = SetupDirectoryAdapter();
		var generator = new ConfigGenerator(file, path, directory);

		// act
		var func = () => generator.Generate(Fixture.Create<string>());

		// assert
		func.Should().NotThrow();
	}

	private IFileAdapter SetupFileAdapter()
	{
		var adapter = new Mock<IFileAdapter>();
		return adapter.Object;
	}

	private IPathAdapter SetupPathAdapter()
	{
		var adapter = new Mock<IPathAdapter>();
		return adapter.Object;
	}

	private IDirectoryAdapter SetupDirectoryAdapter()
	{
		var adapter = new Mock<IDirectoryAdapter>();
		return adapter.Object;
	}
}
