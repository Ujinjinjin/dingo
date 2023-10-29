using Dingo.Core;
using Dingo.Core.Repository;
using Dingo.Core.Repository.Source;
using Dingo.Core.Services.Adapters;
using Microsoft.Data.SqlClient;
using Npgsql;
using Trico.Configuration;

namespace Dingo.UnitTests.Database;

public class ConnectionFactoryTests : UnitTestBase
{
	private const string ValidConnectionString = "Server=hostname;Database=db;User Id=user;Password=pwd;";

	[Theory]
	[InlineData(ProviderName.SqlServer, typeof(SqlConnection))]
	[InlineData(ProviderName.Postgres, typeof(NpgsqlConnection))]
	[InlineData(ProviderName.PostgreSql, typeof(NpgsqlConnection))]
	public void ConnectionFactoryTests_Create__WhenValidProviderNameGiven_ThenConnectionCreated(string providerName, Type expectedType)
	{
		// arrange
		var configuration = SetupConfiguration(providerName, ValidConnectionString);
		var dataSourceProvider = SetupDataSourceProvider(ValidConnectionString);
		var factory = new ConnectionFactory(configuration, dataSourceProvider);

		// act
		var connection = factory.Create();

		// assert
		connection.Should().NotBeNull();
		connection.Should().BeOfType(expectedType);
	}

	[Theory]
	[InlineData(ProviderName.SqlServer, null)]
	[InlineData(ProviderName.SqlServer, "")]
	[InlineData(ProviderName.SqlServer, " ")]
	[InlineData(ProviderName.Postgres, null)]
	[InlineData(ProviderName.Postgres, "")]
	[InlineData(ProviderName.Postgres, " ")]
	[InlineData(ProviderName.PostgreSql, null)]
	[InlineData(ProviderName.PostgreSql, "")]
	[InlineData(ProviderName.PostgreSql, " ")]
	public void ConnectionFactoryTests_Create__WhenEmptyOrNullConnectionStringGiven_ThenExceptionThrown(string providerName, string connectionString)
	{
		// arrange
		var configuration = SetupConfiguration(providerName, connectionString);
		var dataSourceProvider = SetupDataSourceProvider(connectionString);
		var factory = new ConnectionFactory(configuration, dataSourceProvider);

		// act
		var func = () => factory.Create();

		// assert
		func.Should().Throw<ArgumentNullException>();
	}

	[Theory]
	[InlineData(ProviderName.SqlServer)]
	[InlineData(ProviderName.Postgres)]
	[InlineData(ProviderName.PostgreSql)]
	public void ConnectionFactoryTests_Create__WhenInvalidConnectionStringGiven_ThenExceptionThrown(string providerName)
	{
		// arrange
		var connectionString = Fixture.Create<string>();
		var configuration = SetupConfiguration(providerName, connectionString);
		var dataSourceProvider = SetupDataSourceProvider(connectionString);
		var factory = new ConnectionFactory(configuration, dataSourceProvider);

		// act
		var func = () => factory.Create();

		// assert
		func.Should().Throw<ArgumentException>();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void ConnectionFactoryTests_Create__WhenEmptyOrNullProviderNameGiven_ThenExceptionThrown(string providerName)
	{
		// arrange
		var configuration = SetupConfiguration(providerName, ValidConnectionString);
		var dataSourceProvider = SetupDataSourceProvider(ValidConnectionString);
		var factory = new ConnectionFactory(configuration, dataSourceProvider);

		// act
		var func = () => factory.Create();

		// assert
		func.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void ConnectionFactoryTests_Create__WhenInvalidProviderNameGiven_ThenExceptionThrown()
	{
		// arrange
		var configuration = SetupConfiguration(Fixture.Create<string>(), ValidConnectionString);
		var dataSourceProvider = SetupDataSourceProvider(ValidConnectionString);
		var factory = new ConnectionFactory(configuration, dataSourceProvider);

		// act
		var func = () => factory.Create();

		// assert
		func.Should().Throw<ArgumentOutOfRangeException>();
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

	private INpgsqlDataSourceProvider SetupDataSourceProvider(string connectionString)
	{
		var provider = new Mock<INpgsqlDataSourceProvider>();
		var dataSource = new Mock<INpgsqlDataSource>();

		if (!string.IsNullOrWhiteSpace(connectionString))
		{
			dataSource.Setup(ds => ds.CreateConnection())
				.Returns(() => new NpgsqlConnection(connectionString));
		}

		provider.Setup(p => p.Instance())
			.Returns(dataSource.Object);

		return provider.Object;
	}
}
