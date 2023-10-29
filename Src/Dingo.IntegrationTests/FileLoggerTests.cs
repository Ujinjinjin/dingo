using AutoFixture;
using Dingo.Core;
using Dingo.Core.Services.Adapters;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Trico.Configuration;

namespace Dingo.IntegrationTests;

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

	private ILogger CreateLogger(LogLevel logLevel)
	{
		var path = ServiceProvider.GetService<IPathAdapter>();
		var loggerFactory = ServiceProvider.GetService<ILoggerFactory>();

		Directory.CreateDirectory(path.GetLogsPath());
		SetLogLevel(logLevel);

		return loggerFactory.CreateLogger<FileLoggerTests>();
	}

	private void SetLogLevel(LogLevel logLevel)
	{
		var configuration = ServiceProvider.GetService<IConfiguration>();
		configuration[Configuration.Key.LogLevel] = logLevel.ToString();
	}

	private string[] GetLogFiles()
	{
		var path = ServiceProvider.GetService<IPathAdapter>();
		return Directory.GetFiles(path.GetLogsPath());
	}
}
