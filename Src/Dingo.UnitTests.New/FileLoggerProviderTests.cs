using Dingo.Core.IO;
using Dingo.Core.Services.Adapters;
using Trico.Configuration;

namespace Dingo.UnitTests;

public class FileLoggerProviderTests : UnitTestBase
{
	[Fact]
	public void FileLoggerProviderTests_CreateLogger__SmokeTest()
	{
		// arrange
		var configuration = SetupConfiguration();
		var file = SetupFileAdapter();
		var directory = SetupDirectoryAdapter();
		var path = SetupPathAdapter();
		var provider = new FileLoggerProvider(configuration, file, directory, path);

		// act
		var func = () => provider.CreateLogger(Fixture.Create<string>());

		// assert
		func.Should().NotThrow();
		func().Should().NotBeNull();
	}

	private IConfiguration SetupConfiguration()
	{
		var configuration = new Mock<IConfiguration>();
		return configuration.Object;
	}

	private IFileAdapter SetupFileAdapter()
	{
		var adapter = new Mock<IFileAdapter>();
		return adapter.Object;
	}

	private IDirectoryAdapter SetupDirectoryAdapter()
	{
		var adapter = new Mock<IDirectoryAdapter>();
		return adapter.Object;
	}

	private IPathAdapter SetupPathAdapter()
	{
		var adapter = new Mock<IPathAdapter>();
		return adapter.Object;
	}
}
