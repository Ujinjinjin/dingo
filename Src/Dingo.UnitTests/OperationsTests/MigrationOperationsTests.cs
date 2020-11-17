using AutoFixture;
using Dingo.Core.Abstractions;
using Dingo.Core.Config;
using Dingo.Core.Helpers;
using Dingo.Core.Models;
using Dingo.Core.Operations;
using Dingo.Core.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Dingo.UnitTests.OperationsTests
{
	public class MigrationOperationsTests : UnitTestsBase
	{
		[Fact]
		public void MigrationOperationsTests__HandshakeDatabaseConnectionAsync__WhenHandshakeEstablished_ThenInfoMessageShown()
		{
			// Arrange
			var configWrapperMock = new Mock<IConfigWrapper>();
			var rendererMock = new Mock<IRenderer>();
			var databaseHelperMock = new Mock<IDatabaseRepository>();

			var fixture = CreateFixture(configWrapperMock, rendererMock, databaseHelperMock);
			
			databaseHelperMock
				.Setup(x => x.HandshakeDatabaseConnectionAsync())
				.Returns(Task.FromResult(true));

			var providerOperations = fixture.Create<MigrationOperations>();

			// Act
			providerOperations.HandshakeDatabaseConnectionAsync().Wait();

			// Assert
			configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Info)), Times.Once());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Never());
		}
		
		[Fact]
		public void MigrationOperationsTests__HandshakeDatabaseConnectionAsync__WhenHandshakeNotEstablished_ThenErrorMessageShown()
		{
			// Arrange
			var configWrapperMock = new Mock<IConfigWrapper>();
			var rendererMock = new Mock<IRenderer>();
			var databaseHelperMock = new Mock<IDatabaseRepository>();

			var fixture = CreateFixture(configWrapperMock, rendererMock, databaseHelperMock);
			
			databaseHelperMock
				.Setup(x => x.HandshakeDatabaseConnectionAsync())
				.Returns(Task.FromResult(false));

			var providerOperations = fixture.Create<MigrationOperations>();

			// Act
			providerOperations.HandshakeDatabaseConnectionAsync().Wait();

			// Assert
			configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Info)), Times.Never());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Once());
		}

		[Fact]
		public void MigrationOperationsTests__ShowMigrationsStatusAsync__WhenHandshakeNotEstablished_ThenErrorMessageShownAndCommandTerminated()
		{
			// Arrange
			var configWrapperMock = new Mock<IConfigWrapper>();
			var rendererMock = new Mock<IRenderer>();
			var databaseHelperMock = new Mock<IDatabaseRepository>();

			var fixture = CreateFixture(configWrapperMock, rendererMock, databaseHelperMock);
			
			databaseHelperMock
				.Setup(x => x.HandshakeDatabaseConnectionAsync())
				.Returns(Task.FromResult(false));

			var providerOperations = fixture.Create<MigrationOperations>();

			// Act
			providerOperations.ShowMigrationsStatusAsync(
				fixture.Create<string>()
			).Wait();

			// Assert
			configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Once());
			rendererMock.Verify(x => x.ShowMigrationsStatusAsync(It.IsAny<IList<MigrationInfo>>(), It.IsAny<bool>()), Times.Never());
		}

		[Fact]
		public void MigrationOperationsTests__ShowMigrationsStatusAsync__WhenHandshakeEstablished_ThenCommandExecuted()
		{
			// Arrange
			var configWrapperMock = new Mock<IConfigWrapper>();
			var rendererMock = new Mock<IRenderer>();
			var databaseHelperMock = new Mock<IDatabaseRepository>();

			var fixture = CreateFixture(configWrapperMock, rendererMock, databaseHelperMock);
			
			databaseHelperMock
				.Setup(x => x.HandshakeDatabaseConnectionAsync())
				.Returns(Task.FromResult(true));

			var providerOperations = fixture.Create<MigrationOperations>();

			// Act
			providerOperations.ShowMigrationsStatusAsync(
				fixture.Create<string>()
			).Wait();

			// Assert
			configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Never());
			rendererMock.Verify(x => x.ShowMigrationsStatusAsync(It.IsAny<IList<MigrationInfo>>(), It.IsAny<bool>()), Times.Once());
		}

		[Fact]
		public void MigrationOperationsTests__RunMigrationsAsync__WhenHandshakeNotEstablished_ThenErrorMessageShownAndCommandTerminated()
		{
			// Arrange
			var configWrapperMock = new Mock<IConfigWrapper>();
			var rendererMock = new Mock<IRenderer>();
			var databaseHelperMock = new Mock<IDatabaseRepository>();

			var fixture = CreateFixture(configWrapperMock, rendererMock, databaseHelperMock);
			
			databaseHelperMock
				.Setup(x => x.HandshakeDatabaseConnectionAsync())
				.Returns(Task.FromResult(false));

			var providerOperations = fixture.Create<MigrationOperations>();

			// Act
			providerOperations.RunMigrationsAsync(
				fixture.Create<string>()
			).Wait();

			// Assert
			configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Once());
			rendererMock.Verify(x => x.ShowMigrationsStatusAsync(It.IsAny<IList<MigrationInfo>>(), It.IsAny<bool>()), Times.Never());
		}

		[Fact]
		public void MigrationOperationsTests__RunMigrationsAsync__WhenHandshakeEstablished_ThenCommandExecuted()
		{
			// Arrange
			var configWrapperMock = new Mock<IConfigWrapper>();
			var rendererMock = new Mock<IRenderer>();
			var databaseHelperMock = new Mock<IDatabaseRepository>();
			var hashMakerMock = new Mock<IHashMaker>();

			var fixture = CreateFixture(configWrapperMock, rendererMock, databaseHelperMock, hashMakerMock);
			
			var migrationInfoList = CreateItemArray<MigrationInfo>(fixture.Create<int>());
			
			databaseHelperMock
				.Setup(x => x.HandshakeDatabaseConnectionAsync())
				.Returns(Task.FromResult(true));
			databaseHelperMock
				.Setup(x => x.GetMigrationsStatusAsync(It.IsAny<IList<MigrationInfo>>()))
				.Returns(Task.FromResult(migrationInfoList));
			hashMakerMock
				.Setup(x => x.GetMigrationInfoList(It.IsAny<IList<FilePath>>()))
				.Returns(migrationInfoList);

			var providerOperations = fixture.Create<MigrationOperations>();

			// Act
			providerOperations.RunMigrationsAsync(
				fixture.Create<string>()
			).Wait();

			// Assert
			configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
			rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Never());
		}
	}
}
