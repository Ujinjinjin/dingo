using AutoFixture;
using AutoFixture.AutoMoq;
using Dingo.Core.Config;
using Dingo.Core.Constants;
using Dingo.Core.Extensions;
using Dingo.Core.Factories;
using Dingo.Core.Repository;
using Dingo.UnitTests.Base;
using Moq;
using System;
using Xunit;

namespace Dingo.UnitTests.FactoryTests
{
	public class DatabaseContextFactoryTests : UnitTestsBase
	{
		[Fact]
		public void DatabaseContextFactoryTests__CreateDatabaseContext__WhenSupportedDbProviderNameGiven_ThenDatabaseContextCreated()
		{
			// Arrange
			var fixture = new Fixture()
				.Customize(new AutoMoqCustomization());

			var configWrapperMock = new Mock<IConfigWrapper>();
			configWrapperMock
				.Setup(x => x.ProviderName)
				.Returns(DbProvider.SupportedDatabaseProviderNames.GetRandom());
			configWrapperMock
				.Setup(x => x.ConnectionString)
				.Returns(fixture.Create<string>());
			
			fixture.Register(() => configWrapperMock.Object);

			var databaseContextFactory = fixture.Create<DatabaseContextFactory>();

			// Act
			var databaseContext = databaseContextFactory.CreateDatabaseContext();

			// Assert
			Assert.IsAssignableFrom<IDatabaseContext>(databaseContext);
		}
		
		[Fact]
		public void DatabaseContextFactoryTests__CreateDatabaseContext__WhenNotSupportedDbProviderNameGiven_ThenExceptionThrown()
		{
			// Arrange
			var fixture = new Fixture()
				.Customize(new AutoMoqCustomization());

			var configWrapperMock = new Mock<IConfigWrapper>();
			configWrapperMock
				.Setup(x => x.ProviderName)
				.Returns(fixture.Create<string>());
			configWrapperMock
				.Setup(x => x.ConnectionString)
				.Returns(fixture.Create<string>());
			
			fixture.Register(() => configWrapperMock.Object);

			var databaseContextFactory = fixture.Create<DatabaseContextFactory>();

			// Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => databaseContextFactory.CreateDatabaseContext());
		}
	}
}
