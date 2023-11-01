using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace Dingo.IntegrationTests.IO;

public class FileLoggerTests : IntegrationTestBase
{
	[Theory]
	[InlineData(LogLevel.Trace)]
	[InlineData(LogLevel.Debug)]
	[InlineData(LogLevel.Information)]
	[InlineData(LogLevel.Warning)]
	[InlineData(LogLevel.Error)]
	[InlineData(LogLevel.Critical)]
	public async Task FileLoggerTests_Log__WhenGivenMessage_ThenLogContainsMessage(LogLevel logLevel)
	{
		// arrange
		var logger = CreateLogger(LogLevel.Trace);
		var message = Fixture.Create<string>();

		// act
		logger.Log(logLevel, message);

		// assert
		var logFiles = GetLogFiles();
		logFiles.Should().NotBeEmpty();

		var logFilePath = logFiles.Last();
		var logContents = await File.ReadAllTextAsync(logFilePath);
		logContents.Should().NotBeEmpty();
		logContents.Should().Contain(message);
	}

	[Theory]
	[InlineData(LogLevel.Trace)]
	[InlineData(LogLevel.Debug)]
	[InlineData(LogLevel.Information)]
	[InlineData(LogLevel.Warning)]
	[InlineData(LogLevel.Error)]
	[InlineData(LogLevel.Critical)]
	public async Task FileLoggerTests_Log__WhenGivenLogLevel_ThenLogContainsLogLevel(LogLevel logLevel)
	{
		// arrange
		var logger = CreateLogger(LogLevel.Trace);
		var message = Fixture.Create<string>();

		// act
		logger.Log(logLevel, message);

		// assert
		var logFiles = GetLogFiles();
		logFiles.Should().NotBeEmpty();

		var logFilePath = logFiles.Last();
		var logContents = await File.ReadAllTextAsync(logFilePath);

		var logLines = logContents.Split(Environment.NewLine);
		var logLine = logLines.FirstOrDefault(x => x.Contains(message));
		logLine.Should().NotBeNull();
		logLine.Should().Contain(logLevel.ToString());
	}

	[Theory]
	[InlineData(LogLevel.Trace)]
	[InlineData(LogLevel.Debug)]
	[InlineData(LogLevel.Information)]
	[InlineData(LogLevel.Warning)]
	[InlineData(LogLevel.Error)]
	[InlineData(LogLevel.Critical)]
	public async Task FileLoggerTests_Log__WhenLogIsDisabled_ThenNotLogged(LogLevel logLevel)
	{
		// arrange
		var logger = CreateLogger(LogLevel.None);
		var message = Fixture.Create<string>();

		// act
		logger.Log(logLevel, message);

		// assert
		var logFiles = GetLogFiles();
		if (logFiles.Length > 0)
		{
			logFiles.Should().NotBeEmpty();

			var logFilePath = logFiles.Last();
			var logContents = await File.ReadAllTextAsync(logFilePath);
			logContents.Should().NotContain(message);
		}
	}
}
