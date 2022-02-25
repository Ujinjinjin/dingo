using AutoFixture;
using Dingo.Core.Config;
using Dingo.Core.Constants;
using Dingo.Core.Extensions;
using Dingo.Core.Factories;
using Dingo.Core.Repository;
using Moq;
using Xunit;

namespace Dingo.UnitTests.FactoryTests;

public class DatabaseContextFactoryTests : UnitTestsBase
{
	[Fact]
	public void DatabaseContextFactoryTests__CreateDatabaseContext__WhenSupportedDbProviderNameGiven_ThenDatabaseContextCreated()
	{
		// Arrange
		var configWrapperMock = new Mock<IConfigWrapper>();

		var fixture = CreateFixture(configWrapperMock);

		configWrapperMock
			.Setup(x => x.ProviderName)
			.Returns(DbProvider.SupportedDatabaseProviderNames.GetRandom());
		configWrapperMock
			.Setup(x => x.ConnectionString)
			.Returns(fixture.Create<string>());

		var databaseContextFactory = fixture.Create<DatabaseContextFactory>();

		// Act
		var databaseContext = databaseContextFactory.CreateDatabaseContext();

		// Assert
		Assert.NotNull(databaseContext);
		Assert.IsAssignableFrom<IDatabaseContext>(databaseContext);
	}

	[Fact]
	public void DatabaseContextFactoryTests__CreateDatabaseContext__WhenNotSupportedDbProviderNameGiven_ThenExceptionThrown()
	{
		// Arrange
		var configWrapperMock = new Mock<IConfigWrapper>();

		var fixture = CreateFixture(configWrapperMock);

		configWrapperMock
			.Setup(x => x.ProviderName)
			.Returns(fixture.Create<string>());
		configWrapperMock
			.Setup(x => x.ConnectionString)
			.Returns(fixture.Create<string>());

		var databaseContextFactory = fixture.Create<DatabaseContextFactory>();

		// Act & Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => databaseContextFactory.CreateDatabaseContext());
	}
}