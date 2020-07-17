using AutoFixture;
using AutoFixture.AutoMoq;
using Dingo.Core.Abstractions;
using Dingo.Core.Config;
using Dingo.Core.Constants;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Operations;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Dingo.UnitTests.OperationsTests
{
	public class ProviderOperationsTests
	{
		[Fact]
		public void ProviderOperationsTests__ListSupportedDatabaseProvidersAsync__SmokeTest()
		{
			// Arrange
			var fixture = new Fixture()
				.Customize(new AutoMoqCustomization());
			
			var rendererMock = new Mock<IRenderer>();
			fixture.Register(() => rendererMock.Object);

			var providerOperations = fixture.Create<ProviderOperations>();

			// Act
			providerOperations.ListSupportedDatabaseProvidersAsync().Wait();

			// Assert
			rendererMock.Verify(x => x.ListItemsAsync(It.Is<IList<string>>(y => y.SequenceEqual(DbProvider.SupportedDatabaseProviderNames))), Times.Once());
		}

		[Fact]
		public void ProviderOperationsTests__ChooseDatabaseProviderAsync__WhenUserChoseProviderFromList_ThenConfigUpdated()
		{
			// Arrange
			var fixture = new Fixture()
				.Customize(new AutoMoqCustomization());

			var userChoice = DbProvider.SupportedDatabaseProviderNames.GetRandom();
			
			var configWrapperMock = new Mock<IConfigWrapper>();
			configWrapperMock.SetupAllProperties();
			fixture.Register(() => configWrapperMock.Object);
			var rendererMock = new Mock<IRenderer>();
			fixture.Register(() => rendererMock.Object);
			var promptMock = new Mock<IPrompt>();
			promptMock
				.Setup(x => x.Choose(It.IsAny<string>(), It.Is<IList<string>>(y => y.SequenceEqual(DbProvider.SupportedDatabaseProviderNames))))
				.Returns(userChoice);
			fixture.Register(() => promptMock.Object);

			var providerOperations = fixture.Create<ProviderOperations>();

			// Act
			providerOperations.ChooseDatabaseProviderAsync().Wait();

			// Assert
			Assert.Equal(userChoice, configWrapperMock.Object.ProviderName);
			configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
			configWrapperMock.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Info)), Times.Once());
			promptMock.Verify(x => x.Choose(It.IsAny<string>(), It.Is<IList<string>>(y => y.SequenceEqual(DbProvider.SupportedDatabaseProviderNames))), Times.Once());
		}

		[Fact]
		public void ProviderOperationsTests__ValidateDatabaseProviderAsync__WhenSupportedProviderNameGiven_ThenInfoMessageShown()
		{
			// Arrange
			var fixture = new Fixture()
				.Customize(new AutoMoqCustomization());

			var configWrapperMock = new Mock<IConfigWrapper>();
			configWrapperMock.SetupAllProperties();
			configWrapperMock.Object.ProviderName = DbProvider.SupportedDatabaseProviderNames.GetRandom();
			fixture.Register(() => configWrapperMock.Object);
			var rendererMock = new Mock<IRenderer>();
			fixture.Register(() => rendererMock.Object);
			
			var providerOperations = fixture.Create<ProviderOperations>();

			// Act
			providerOperations.ValidateDatabaseProviderAsync().Wait();

			// Assert
			configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Info)), Times.Once());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Warning)), Times.Never());
		}

		[Fact]
		public void ProviderOperationsTests__ValidateDatabaseProviderAsync__WhenNotSupportedProviderNameGiven_ThenWarningMessageShown()
		{
			// Arrange
			var fixture = new Fixture()
				.Customize(new AutoMoqCustomization());

			var configWrapperMock = new Mock<IConfigWrapper>();
			configWrapperMock.SetupAllProperties();
			configWrapperMock.Object.ProviderName = fixture.Create<string>();
			fixture.Register(() => configWrapperMock.Object);
			var rendererMock = new Mock<IRenderer>();
			fixture.Register(() => rendererMock.Object);
			
			var providerOperations = fixture.Create<ProviderOperations>();

			// Act
			providerOperations.ValidateDatabaseProviderAsync().Wait();

			// Assert
			configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Info)), Times.Never());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Warning)), Times.Once());
		}
	}
}
