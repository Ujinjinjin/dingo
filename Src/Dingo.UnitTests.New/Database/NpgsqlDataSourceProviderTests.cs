using Dingo.Core;
using Dingo.Core.Exceptions;
using Dingo.Core.Repository.Source;
using Dingo.Core.Services.Adapters;
using Trico.Configuration;

namespace Dingo.UnitTests.Database;

public class NpgsqlDataSourceProviderTests : UnitTestBase
{
	[Theory]
	[InlineData("")]
	public void NpgsqlDataSourceProviderTests_Create__WhenProviderNotPostgres_ThenExceptionThrown(string providerName)
	{
		// arrange
		var configuration = SetupConfiguration(providerName, Fixture.Create<string>());
		var dataSourceBuilder = SetupDataSourceBuilder();
		var loggerFactory = SetupLoggerFactory();

		var dsProvider = new NpgsqlDataSourceProvider(configuration, dataSourceBuilder, loggerFactory);

		// act
		var func = () => dsProvider.Instance();

		// assert
		func.Should().Throw<IncompatibleDatabaseException>();
	}

	private IConfiguration SetupConfiguration(string providerName, string connectionString)
	{
		var config = new Mock<IConfiguration>();
		config.Setup(c => c.Get(It.Is<string>(name => name == Configuration.Key.DatabaseProvider)))
			.Returns(providerName);
		config.Setup(c => c.Get(It.Is<string>(name => name == Configuration.Key.ConnectionString)))
			.Returns(connectionString);

		return config.Object;
	}

	private INpgsqlDataSourceBuilder SetupDataSourceBuilder()
	{
		var provider = new Mock<INpgsqlDataSourceBuilder>();
		return provider.Object;
	}
}
