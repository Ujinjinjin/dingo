using AutoFixture;
using Dingo.Core.Abstractions;
using Dingo.Core.Config;
using Dingo.Core.Helpers;
using Dingo.Core.Models;
using Dingo.Core.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dingo.Core.Services;
using Xunit;

namespace Dingo.UnitTests.OperationsTests;

public class LogsOperationsTests : UnitTestsBase
{
	[Fact]
	public void LogsOperationsTests__SwitchLogLevelAsync__WhenLogLevelGiven_ThenLogLevelSavedWithoutPromptingUser()
	{
		// Arrange
		var configWrapperMock = new Mock<IConfigWrapper>();
		var rendererMock = new Mock<IRenderer>();
		var promptMock = new Mock<IPrompt>();

		var fixture = CreateFixture(configWrapperMock, rendererMock, promptMock);

		var logsOperations = fixture.Create<LogsService>();

		// Act
		logsOperations.SwitchLogLevelAsync(fixture.Create<string>(), fixture.Create<int>()).Wait();

		// Assert
		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		configWrapperMock.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		promptMock.Verify(x => x.Choose(It.IsAny<string>(), It.IsAny<IList<LogLevel>>(), It.IsAny<Func<LogLevel,string>>()), Times.Never());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Info)), Times.Once());
	}

	[Fact]
	public void LogsOperationsTests__SwitchLogLevelAsync__WhenLogLevelNotGiven_ThenLogLevelUserPromptedAndChosenLogLevelSaved()
	{
		// Arrange
		var configWrapperMock = new Mock<IConfigWrapper>();
		var rendererMock = new Mock<IRenderer>();
		var promptMock = new Mock<IPrompt>();

		var fixture = CreateFixture(configWrapperMock, rendererMock, promptMock);

		var logsOperations = fixture.Create<LogsService>();

		// Act
		logsOperations.SwitchLogLevelAsync(fixture.Create<string>(), null).Wait();

		// Assert
		configWrapperMock.Verify(x => x.LoadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		configWrapperMock.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
		promptMock.Verify(x => x.Choose(It.IsAny<string>(), It.IsAny<IList<LogLevel>>(), It.IsAny<Func<LogLevel,string>>()), Times.Once());
		rendererMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.Is<MessageType>(y => y == MessageType.Info)), Times.Once());
	}

	[Fact]
	public void LogsOperationsTests__PruneLogsAsync__WhenLogsPathGiven_ThenAllFilesWithinPathDeleted()
	{
		// Arrange
		var logsDirectory = $"{Directory.GetCurrentDirectory()}/logs";

		var pathHelper = new Mock<IPathHelper>();
		pathHelper
			.Setup(x => x.GetLogsDirectory())
			.Returns(logsDirectory);

		var fixture = CreateFixture(pathHelper);

		var logsOperations = fixture.Create<LogsService>();

		Directory.CreateDirectory($"{logsDirectory}/1");
		Directory.CreateDirectory($"{logsDirectory}/2");
		File.Create($"{logsDirectory}/1/1.log").Close();
		File.Create($"{logsDirectory}/2/2.log").Close();

		// Act
		logsOperations.PruneLogsAsync().Wait();

		// Assert
		var di = new DirectoryInfo(logsDirectory);

		var filesCount = 0;
		foreach (var _ in di.EnumerateFiles())
		{
			filesCount++;
		}

		var dirsCount = 0;
		foreach (var _ in di.EnumerateDirectories())
		{
			dirsCount++;
		}

		Assert.Equal(0, filesCount);
		Assert.Equal(0, dirsCount);
	}

}