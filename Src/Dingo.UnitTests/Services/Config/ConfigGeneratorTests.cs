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

	private IFile SetupFileAdapter()
	{
		var adapter = new Mock<IFile>();
		return adapter.Object;
	}

	private IPath SetupPathAdapter()
	{
		var adapter = new Mock<IPath>();
		return adapter.Object;
	}

	private IDirectory SetupDirectoryAdapter()
	{
		var adapter = new Mock<IDirectory>();
		return adapter.Object;
	}
}
