using AutoFixture;
using Dingo.Core;
using Dingo.Core.Extensions;
using Dingo.Core.Services.Adapters;
using Dingo.IntegrationTests.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Trico.Configuration;

namespace Dingo.IntegrationTests;

public class IntegrationTestBase
{
	protected readonly Fixture Fixture;
	protected readonly IServiceProvider ServiceProvider;

	protected IntegrationTestBase()
	{
		Fixture = new Fixture();
		ServiceProvider = BuildServiceProvider();

	}

	private IServiceProvider BuildServiceProvider()
	{
		var sc = new ServiceCollection();
		sc.AddDingo();
		var sp = sc.BuildServiceProvider();
		sp.UseDingo();

		return sp;
	}

	protected ILogger CreateLogger(LogLevel logLevel)
	{
		var path = ServiceProvider.GetService<IPath>();
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

	protected string[] GetLogFiles()
	{
		var path = ServiceProvider.GetService<IPath>();
		return Directory.GetFiles(path.GetLogsPath());
	}
}
