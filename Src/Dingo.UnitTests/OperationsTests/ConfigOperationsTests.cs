using AutoFixture;
using Dingo.Core.Abstractions;
using Dingo.Core.Config;
using Dingo.Core.Models;
using Moq;
using System.Threading;
using Dingo.Core.Services;
using Xunit;

namespace Dingo.UnitTests.OperationsTests;

public class ConfigOperationsTests : UnitTestsBase
{
	[Fact]
	public void ConfigOperationsTests__UpdateProjectConfigurationAsync__WhenValidParamsAreGiven_ThenConfigsUpdated()
	{
		// Arrange
		var fixture = CreateFixture();

		var configWrapperMock = new Mock<IConfigWrapper>();
		configWrapperMock.SetupAllProperties();

		fixture.Register(() => configWrapperMock.Object);

		var configOperations = fixture.Create<ConfigService>();

		var connectionString = fixture.Create<string>();
		var providerName = fixture.Create<string>();
		var migrationSchema = fixture.Create<string>();
		var migrationTable = fixture.Create<string>();
		var searchPattern = fixture.Create<string>();

		// Act
		configOperations.UpdateProjectConfigurationAsync(
			fixture.Create<string>(),
			connectionString,
			providerName,
			migrationSchema,
			migrationTable,
			searchPattern
		).Wait();

		// Assert
		Assert.Equal(connectionString, configWrapperMock.Object.ConnectionString);
		Assert.Equal(providerName, configWrapperMock.Object.ProviderName);
		Assert.Equal(migrationSchema, configWrapperMock.Object.MigrationSchema);
		Assert.Equal(migrationTable, configWrapperMock.Object.MigrationTable);
		Assert.Equal(searchPattern, configWrapperMock.Object.MigrationsSearchPattern);

		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		configWrapperMock.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
	}

	[Fact]
	public void ConfigOperationsTests__UpdateProjectConfigurationAsync__WhenNullParamsAreGiven_ThenConfigsNotUpdated()
	{
		// Arrange
		var configWrapperMock = new Mock<IConfigWrapper>();

		var fixture = CreateFixture(configWrapperMock);

		var connectionString = fixture.Create<string>();
		var providerName = fixture.Create<string>();
		var migrationSchema = fixture.Create<string>();
		var migrationTable = fixture.Create<string>();
		var searchPattern = fixture.Create<string>();

		configWrapperMock.SetupAllProperties();
		configWrapperMock.Object.ConnectionString = connectionString;
		configWrapperMock.Object.ProviderName = providerName;
		configWrapperMock.Object.MigrationSchema = migrationSchema;
		configWrapperMock.Object.MigrationTable = migrationTable;
		configWrapperMock.Object.MigrationsSearchPattern = searchPattern;

		fixture.Register(() => configWrapperMock.Object);

		var configOperations = fixture.Create<ConfigService>();

		// Act
		configOperations.UpdateProjectConfigurationAsync(
			fixture.Create<string>()
		).Wait();

		// Assert
		Assert.Equal(connectionString, configWrapperMock.Object.ConnectionString);
		Assert.Equal(providerName, configWrapperMock.Object.ProviderName);
		Assert.Equal(migrationSchema, configWrapperMock.Object.MigrationSchema);
		Assert.Equal(migrationTable, configWrapperMock.Object.MigrationTable);
		Assert.Equal(searchPattern, configWrapperMock.Object.MigrationsSearchPattern);

		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		configWrapperMock.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
	}

	[Fact]
	public void ConfigOperationsTests__ShowProjectConfigurationAsync__SmokeTest()
	{
		// Arrange
		var configWrapperMock = new Mock<IConfigWrapper>();
		var rendererMock = new Mock<IRenderer>();

		var fixture = CreateFixture(configWrapperMock, rendererMock);

		var configOperations = fixture.Create<ConfigService>();

		// Act
		configOperations.ShowProjectConfigurationAsync(
			fixture.Create<string>()
		).Wait();

		// Assert
		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		rendererMock.Verify(x => x.ShowConfigAsync(It.Is<IConfigWrapper>(wrapper => wrapper == configWrapperMock.Object)), Times.Once());
	}

	[Fact]
	public void ConfigOperationsTests__InitConfigurationFileAsync__WhenFileExistsAndUserChoseToOverride_ThenConfigInitialized()
	{
		// Arrange
		var configWrapperMock = new Mock<IConfigWrapper>();
		var rendererMock = new Mock<IRenderer>();
		var promptMock = new Mock<IPrompt>();

		var fixture = CreateFixture(configWrapperMock, rendererMock, promptMock);

		configWrapperMock.SetupAllProperties();
		configWrapperMock
			.Setup(x => x.ConfigFileExists)
			.Returns(true);
		promptMock
			.Setup(x => x.Confirm(It.IsAny<string>(), It.IsAny<bool?>()))
			.Returns(true);

		var configOperations = fixture.Create<ConfigService>();

		// Act
		configOperations.InitConfigurationFileAsync(
			fixture.Create<string>()
		).Wait();

		// Assert
		Assert.Equal(string.Empty, configWrapperMock.Object.ConnectionString);
		Assert.Equal(string.Empty, configWrapperMock.Object.ProviderName);
		Assert.Null(configWrapperMock.Object.MigrationSchema);
		Assert.Null(configWrapperMock.Object.MigrationTable);
		Assert.Null(configWrapperMock.Object.MigrationsSearchPattern);

		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		configWrapperMock.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		promptMock.Verify(x => x.Confirm(It.IsAny<string>(), It.IsAny<bool?>()), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Info)), Times.Once());
	}

	[Fact]
	public void ConfigOperationsTests__InitConfigurationFileAsync__WhenFileExistsAndUserChoseNotToOverride_ThenConfigNotInitialized()
	{
		// Arrange
		var configWrapperMock = new Mock<IConfigWrapper>();
		var rendererMock = new Mock<IRenderer>();
		var promptMock = new Mock<IPrompt>();

		var fixture = CreateFixture(configWrapperMock, rendererMock, promptMock);

		configWrapperMock.SetupAllProperties();
		configWrapperMock
			.Setup(x => x.ConfigFileExists)
			.Returns(true);

		var configOperations = fixture.Create<ConfigService>();

		// Act
		configOperations.InitConfigurationFileAsync(
			fixture.Create<string>()
		).Wait();

		// Assert
		Assert.Null(configWrapperMock.Object.ConnectionString);
		Assert.Null(configWrapperMock.Object.ProviderName);
		Assert.Null(configWrapperMock.Object.MigrationSchema);
		Assert.Null(configWrapperMock.Object.MigrationTable);
		Assert.Null(configWrapperMock.Object.MigrationsSearchPattern);

		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		configWrapperMock.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
		promptMock.Verify(x => x.Confirm(It.IsAny<string>(), It.IsAny<bool?>()), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.IsAny<MessageType>()), Times.Never());
	}

	[Fact]
	public void ConfigOperationsTests__InitConfigurationFileAsync__WhenFileNotExists_ThenConfigInitializedWithoutUserConfirmation()
	{
		// Arrange
		var configWrapperMock = new Mock<IConfigWrapper>();
		var rendererMock = new Mock<IRenderer>();
		var promptMock = new Mock<IPrompt>();

		var fixture = CreateFixture(configWrapperMock, rendererMock, promptMock);

		configWrapperMock.SetupAllProperties();
		configWrapperMock
			.Setup(x => x.ConfigFileExists)
			.Returns(false);

		var configOperations = fixture.Create<ConfigService>();

		// Act
		configOperations.InitConfigurationFileAsync(
			fixture.Create<string>()
		).Wait();

		// Assert
		Assert.Equal(string.Empty, configWrapperMock.Object.ConnectionString);
		Assert.Equal(string.Empty, configWrapperMock.Object.ProviderName);
		Assert.Null(configWrapperMock.Object.MigrationSchema);
		Assert.Null(configWrapperMock.Object.MigrationTable);
		Assert.Null(configWrapperMock.Object.MigrationsSearchPattern);

		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		configWrapperMock.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		promptMock.Verify(x => x.Confirm(It.IsAny<string>(), It.IsAny<bool?>()), Times.Never());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Info)), Times.Once());
	}
}