using System.Data;
using Dingo.Core.Repository.UoW;

namespace Dingo.UnitTests.Database;

public class UnitOfWorkFactoryTests : UnitTestBase
{
	[Fact]
	public async Task UnitOfWorkFactoryTests_CreateAsync__SmokeTest()
	{
		// arrange
		var connectionFactory = SetupConnectionFactory(MockConnection(ConnectionState.Open));
		var factory = new UnitOfWorkFactory(connectionFactory);

		// act
		await using var unitOfWork = await factory.CreateAsync();

		// assert
		unitOfWork.Should().NotBeNull();
	}

	[Fact]
	public async Task UnitOfWorkFactoryTests_TryGet__WhenCreatedNestedUnits_ThenReturnsTheLastCreatedUnit()
	{
		// arrange
		var connectionFactory = SetupConnectionFactory(MockConnection(ConnectionState.Open));
		var factory = new UnitOfWorkFactory(connectionFactory);

		// act
		await using var rootUnit = await factory.CreateAsync();
		await using var childUnit = await factory.CreateAsync();

		var result = factory.TryGet(out var currentUnitOfWork);

		// assert
		result.Should().BeTrue();
		currentUnitOfWork.Should().NotBeNull();
		currentUnitOfWork!.Id.Should().Be(childUnit.Id);
	}

	[Fact]
	public async Task UnitOfWorkFactoryTests_TryGet__WhenNestedUnitIsClosed_ThenReturnsTheClosestParentUnit()
	{
		// arrange
		var connectionFactory = SetupConnectionFactory(MockConnection(ConnectionState.Open));
		var factory = new UnitOfWorkFactory(connectionFactory);

		// act
		await using var rootUnit = await factory.CreateAsync();
		await using (var _ = await factory.CreateAsync()) {}

		var result = factory.TryGet(out var currentUnitOfWork);

		// assert
		result.Should().BeTrue();
		currentUnitOfWork.Should().NotBeNull();
		currentUnitOfWork!.Id.Should().Be(rootUnit.Id);
	}

	[Fact]
	public async Task UnitOfWorkFactoryTests_TryGet__WhenAllUnitsAreClosed_ThenNoUnitsReturned()
	{
		// arrange
		var connectionFactory = SetupConnectionFactory(MockConnection(ConnectionState.Open));
		var factory = new UnitOfWorkFactory(connectionFactory);

		// act
		await using (var _ = await factory.CreateAsync()) {}
		await using (var _ = await factory.CreateAsync()) {}

		var result = factory.TryGet(out var currentUnitOfWork);

		// assert
		result.Should().BeFalse();
		currentUnitOfWork.Should().BeNull();
	}

	[Fact]
	public void UnitOfWorkFactoryTests_TryGet__WhenNoUnitsCreated_ThenNoUnitsReturned()
	{
		// arrange
		var connectionFactory = SetupConnectionFactory(MockConnection(ConnectionState.Open));
		var factory = new UnitOfWorkFactory(connectionFactory);

		// act
		var result = factory.TryGet(out var currentUnitOfWork);

		// assert
		result.Should().BeFalse();
		currentUnitOfWork.Should().BeNull();
	}
}
