using System.Data;
using Dingo.Core.Repository;
using Dingo.Core.Repository.Command;
using Dingo.UnitTests.Helpers;

namespace Dingo.UnitTests.Database;

public class DatabaseRepositoryTests : UnitTestBase
{
	[Fact]
	public async Task DatabaseRepositoryTests_TryHandshake__WhenConnectionIsAlreadyOpen_ThenHandshakeSucceeded()
	{
		// arrange
		var connectionFactory = SetupConnectionFactory(MockConnection(ConnectionState.Open));
		var commandProviderFactory = SetupCommandProviderFactory();
		var loggerFactory = SetupLoggerFactory();
		var repository = new DatabaseRepository(connectionFactory, commandProviderFactory, loggerFactory);

		// act
		var result = await repository.TryHandshakeAsync();

		// assert
		result.Should().BeTrue();
	}

	[Fact]
	public async Task DatabaseRepositoryTests_TryHandshake__WhenConnectionOpenedSuccessfully_ThenHandshakeSucceeded()
	{
		// arrange
		var connectionFactory = SetupConnectionFactory(MockConnection(ConnectionState.Closed));
		var commandProviderFactory = SetupCommandProviderFactory();
		var loggerFactory = SetupLoggerFactory();
		var repository = new DatabaseRepository(connectionFactory, commandProviderFactory, loggerFactory);

		// act
		var result = await repository.TryHandshakeAsync();

		// assert
		result.Should().BeTrue();
	}

	[Fact]
	public async Task DatabaseRepositoryTests_TryHandshake__WhenConnectionNotOpened_ThenHandshakeFailed()
	{
		// arrange
		var connectionFactory = SetupConnectionFactory(MockConnection(ConnectionState.Closed, false));
		var commandProviderFactory = SetupCommandProviderFactory();
		var loggerFactory = SetupLoggerFactory();
		var repository = new DatabaseRepository(connectionFactory, commandProviderFactory, loggerFactory);

		// act
		var result = await repository.TryHandshakeAsync();

		// assert
		result.Should().BeFalse();
	}

	private IConnectionFactory SetupConnectionFactory(IDbConnection connection)
	{
		var factory = new Mock<IConnectionFactory>();
		factory.Setup(f => f.Create())
			.Returns(() => new DummyConnection(connection));

		return factory.Object;
	}

	private IDbConnection MockConnection(ConnectionState state, bool succeed = true)
	{
		var connection = new Mock<IDbConnection>();

		connection.Setup(c => c.State)
			.Returns(state);

		if (!succeed)
		{
			connection.Setup(c => c.Open())
				.Throws<Exception>();
		}

		return connection.Object;
	}

	private ICommandProviderFactory SetupCommandProviderFactory()
	{
		var factory = new Mock<ICommandProviderFactory>();

		return factory.Object;
	}
}
