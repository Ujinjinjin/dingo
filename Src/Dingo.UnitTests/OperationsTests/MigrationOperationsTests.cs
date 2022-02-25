using AutoFixture;
using Dingo.Core.Abstractions;
using Dingo.Core.Adapters;
using Dingo.Core.Config;
using Dingo.Core.Helpers;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dingo.Core.Services;
using Xunit;

namespace Dingo.UnitTests.OperationsTests;

public class MigrationOperationsTests : UnitTestsBase
{
	[Fact]
	public void MigrationOperationsTests__CreateMigrationFileAsync__WhenDirectoryNotExists_ThenDirectoryCreatedAndFileCreated()
	{
		// Arrange
		var directoryAdapterMock = new Mock<IDirectoryAdapter>();
		var fileAdapterMock = new Mock<IFileAdapter>();
		var rendererMock = new Mock<IRenderer>();

		var fixture = CreateFixture(directoryAdapterMock, fileAdapterMock, rendererMock);

		directoryAdapterMock
			.Setup(x => x.Exists(It.IsAny<string>()))
			.Returns(false);

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.CreateMigrationFileAsync(CreateValidFilename(), fixture.Create<string>()).Wait();

		// Assert
		directoryAdapterMock.Verify(x => x.CreateDirectory(It.IsAny<string>()), Times.Once());
		fileAdapterMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Never());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Warning)), Times.Never());
	}
		
	[Fact]
	public void MigrationOperationsTests__CreateMigrationFileAsync__WhenDirectoryExists_ThenOnlyFileCreated()
	{
		// Arrange
		var directoryAdapterMock = new Mock<IDirectoryAdapter>();
		var fileAdapterMock = new Mock<IFileAdapter>();
		var rendererMock = new Mock<IRenderer>();

		var fixture = CreateFixture(directoryAdapterMock, fileAdapterMock, rendererMock);

		directoryAdapterMock
			.Setup(x => x.Exists(It.IsAny<string>()))
			.Returns(true);

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.CreateMigrationFileAsync(CreateValidFilename(), fixture.Create<string>()).Wait();

		// Assert
		directoryAdapterMock.Verify(x => x.CreateDirectory(It.IsAny<string>()), Times.Never());
		fileAdapterMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Never());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Warning)), Times.Never());
	}
		
	[Fact]
	public void MigrationOperationsTests__CreateMigrationFileAsync__WhenCreateFileThrowsAnError_ThenErrorDisplayedButCommandCompleted()
	{
		// Arrange
		var directoryAdapterMock = new Mock<IDirectoryAdapter>();
		var fileAdapterMock = new Mock<IFileAdapter>();
		var rendererMock = new Mock<IRenderer>();

		var fixture = CreateFixture(directoryAdapterMock, fileAdapterMock, rendererMock);

		directoryAdapterMock
			.Setup(x => x.Exists(It.IsAny<string>()))
			.Returns(true);
		fileAdapterMock
			.Setup(x => x.Create(It.IsAny<string>()))
			.Throws(new Exception());

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.CreateMigrationFileAsync(CreateValidFilename(), fixture.Create<string>()).Wait();

		// Assert
		directoryAdapterMock.Verify(x => x.CreateDirectory(It.IsAny<string>()), Times.Never());
		fileAdapterMock.Verify(x => x.Create(It.IsAny<string>()), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Warning)), Times.Never());
	}
		
	[Fact]
	public void MigrationOperationsTests__CreateMigrationFileAsync__WhenInvalidFilenameGiven_ThenWarningDisplayedAndFileNotCreated()
	{
		// Arrange
		var directoryAdapterMock = new Mock<IDirectoryAdapter>();
		var fileAdapterMock = new Mock<IFileAdapter>();
		var rendererMock = new Mock<IRenderer>();

		var fixture = CreateFixture(directoryAdapterMock, fileAdapterMock, rendererMock);

		directoryAdapterMock
			.Setup(x => x.Exists(It.IsAny<string>()))
			.Returns(true);
		fileAdapterMock
			.Setup(x => x.Create(It.IsAny<string>()))
			.Throws(new Exception());

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.CreateMigrationFileAsync(fixture.Create<string>(), fixture.Create<string>()).Wait();

		// Assert
		directoryAdapterMock.Verify(x => x.CreateDirectory(It.IsAny<string>()), Times.Never());
		fileAdapterMock.Verify(x => x.Create(It.IsAny<string>()), Times.Never());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Never());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Warning)), Times.Once());
	}

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

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.HandshakeDatabaseConnectionAsync().Wait();

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

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.HandshakeDatabaseConnectionAsync().Wait();

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

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.ShowMigrationsStatusAsync(
			fixture.Create<string>()
		).Wait();

		// Assert
		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Once());
		rendererMock.Verify(x => x.ShowMigrationsStatusAsync(It.IsAny<IList<MigrationInfo>>(), It.IsAny<bool>()), Times.Never());
	}

	[Fact]
	public void MigrationOperationsTests__ShowMigrationsStatusAsync__WhenAnyMigrationHasInvalidFilename_ThenWarningMessageShownAndCommandTerminated()
	{
		// Arrange
		var configWrapperMock = new Mock<IConfigWrapper>();
		var rendererMock = new Mock<IRenderer>();
		var databaseHelperMock = new Mock<IDatabaseRepository>();
		var directoryScannerMock = new Mock<IDirectoryScanner>();

		var fixture = CreateFixture(configWrapperMock, rendererMock, databaseHelperMock, directoryScannerMock);

		databaseHelperMock
			.Setup(x => x.HandshakeDatabaseConnectionAsync())
			.Returns(Task.FromResult(true));

		directoryScannerMock
			.Setup(x => x.GetFilePathList(It.IsAny<string>(), It.IsAny<string>()))
			.Returns(CreateItemArray<FilePath>(10));

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.ShowMigrationsStatusAsync(
			fixture.Create<string>()
		).Wait();

		// Assert
		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Never());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Warning)), Times.Once());
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

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.ShowMigrationsStatusAsync(
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

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.RunMigrationsAsync(
			fixture.Create<string>()
		).Wait();

		// Assert
		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Once());
		rendererMock.Verify(x => x.ShowMigrationsStatusAsync(It.IsAny<IList<MigrationInfo>>(), It.IsAny<bool>()), Times.Never());
	}

	[Fact]
	public void MigrationOperationsTests__RunMigrationsAsync__WhenAnyMigrationHasInvalidFilename_ThenWarningMessageShownAndCommandTerminated()
	{
		// Arrange
		var configWrapperMock = new Mock<IConfigWrapper>();
		var rendererMock = new Mock<IRenderer>();
		var databaseHelperMock = new Mock<IDatabaseRepository>();
		var directoryScannerMock = new Mock<IDirectoryScanner>();

		var fixture = CreateFixture(configWrapperMock, rendererMock, databaseHelperMock, directoryScannerMock);

		databaseHelperMock
			.Setup(x => x.HandshakeDatabaseConnectionAsync())
			.Returns(Task.FromResult(true));

		directoryScannerMock
			.Setup(x => x.GetFilePathList(It.IsAny<string>(), It.IsAny<string>()))
			.Returns(CreateItemArray<FilePath>(10));

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.RunMigrationsAsync(
			fixture.Create<string>()
		).Wait();

		// Assert
		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Never());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Warning)), Times.Once());
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
			.Setup(x => x.GetMigrationInfoListAsync(It.IsAny<IList<FilePath>>()))
			.Returns(Task.FromResult(migrationInfoList));

		var migrationOperations = fixture.Create<MigrationService>();

		// Act
		migrationOperations.RunMigrationsAsync(
			fixture.Create<string>()
		).Wait();

		// Assert
		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Error)), Times.Never());
	}
}