using System.Data;
using Dingo.Core.Repository;
using Dingo.Core.Repository.Command;
using Dingo.Core.Repository.Mapper;
using Dingo.Core.Repository.UoW;
using Trico.Configuration;

namespace Dingo.UnitTests.Database;

public class DatabaseRepositoryTests : UnitTestBase
{
	[Fact]
	public async Task DatabaseRepositoryTests_TryHandshake__WhenConnectionIsAlreadyOpen_ThenHandshakeSucceeded()
	{
		// arrange
		var connectionResolverFactory = SetupConnectionResolverFactory(MockConnection(ConnectionState.Open));
		var commandProviderFactory = SetupCommandProviderFactory();
		var configuration = SetupConfiguration();
		var mapper = SetupMapper();
		var loggerFactory = SetupLoggerFactory();
		var repository = new DatabaseRepository(
			connectionResolverFactory,
			commandProviderFactory,
			configuration,
			mapper,
			loggerFactory
		);

		// act
		var result = await repository.TryHandshakeAsync();

		// assert
		result.Should().BeTrue();
	}

	[Fact]
	public async Task DatabaseRepositoryTests_TryHandshake__WhenConnectionOpenedSuccessfully_ThenHandshakeSucceeded()
	{
		// arrange
		var connectionResolverFactory = SetupConnectionResolverFactory(MockConnection(ConnectionState.Closed));
		var commandProviderFactory = SetupCommandProviderFactory();
		var configuration = SetupConfiguration();
		var mapper = SetupMapper();
		var loggerFactory = SetupLoggerFactory();
		var repository = new DatabaseRepository(
			connectionResolverFactory,
			commandProviderFactory,
			configuration,
			mapper,
			loggerFactory
		);

		// act
		var result = await repository.TryHandshakeAsync();

		// assert
		result.Should().BeTrue();
	}

	[Fact]
	public async Task DatabaseRepositoryTests_TryHandshake__WhenConnectionNotOpened_ThenHandshakeFailed()
	{
		// arrange
		var connectionResolverFactory = SetupConnectionResolverFactory(MockConnection(ConnectionState.Closed, false));
		var commandProviderFactory = SetupCommandProviderFactory();
		var configuration = SetupConfiguration();
		var mapper = SetupMapper();
		var loggerFactory = SetupLoggerFactory();
		var repository = new DatabaseRepository(
			connectionResolverFactory,
			commandProviderFactory,
			configuration,
			mapper,
			loggerFactory
		);

		// act
		var result = await repository.TryHandshakeAsync();

		// assert
		result.Should().BeFalse();
	}

	private IConnectionResolverFactory SetupConnectionResolverFactory(IDbConnection connection)
	{
		var connectionFactory = SetupConnectionFactory(connection);

		var factory = new Mock<IConnectionResolverFactory>();
		factory.Setup(f => f.Create())
			.Returns(() => new ConnectionResolver(new Mock<IUnitOfWorkFactory>().Object, connectionFactory));

		return factory.Object;
	}

	private ICommandProviderFactory SetupCommandProviderFactory()
	{
		var factory = new Mock<ICommandProviderFactory>();

		return factory.Object;
	}

	private IConfiguration SetupConfiguration()
	{
		var config = new Mock<IConfiguration>();

		return config.Object;
	}

	private IDbModelMapper SetupMapper()
	{
		var config = new Mock<IDbModelMapper>();

		return config.Object;
	}
}
