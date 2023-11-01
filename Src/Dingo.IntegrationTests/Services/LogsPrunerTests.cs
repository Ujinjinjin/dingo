using AutoFixture;
using Dingo.Core.Services.Logs;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dingo.IntegrationTests.Services;

public class LogsPrunerTests : IntegrationTestBase
{
	[Fact]
	public void LogsPrunerTests_Prune__WhenExistingLogsPruned_ThenAllLogFilesRemoved()
	{
		// arrange
		var logger = CreateLogger(LogLevel.Trace);
		var message = Fixture.Create<string>();
		var pruner = ServiceProvider.GetService<ILogsPruner>();

		logger.Log(LogLevel.Information, message);

		// act
		pruner.Prune();

		// assert
		var logFiles = GetLogFiles();
		logFiles.Should().BeEmpty();
	}
}
