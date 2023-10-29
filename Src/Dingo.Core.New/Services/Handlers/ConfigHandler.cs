using Dingo.Core.Extensions;
using Dingo.Core.IO;
using Dingo.Core.Services.Config;
using Dingo.Core.Utils;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Services.Handlers;

internal class ConfigHandler : IConfigHandler
{
	private readonly IConfigGenerator _configGenerator;
	private readonly IOutput _output;
	private readonly ILogger _logger;

	public ConfigHandler(
		IConfigGenerator configGenerator,
		IOutput output,
		ILoggerFactory loggerFactory
	)
	{
		_configGenerator = configGenerator.Required(nameof(configGenerator));
		_output = output.Required(nameof(output));
		_logger = loggerFactory.Required(nameof(loggerFactory))
			.CreateLogger<MigrationHandler>()
			.Required(nameof(loggerFactory));
	}

	public void Init(string? path = default, string? profile = default)
	{
		using var _ = new CodeTiming(_logger);

		try
		{
			_configGenerator.Generate(path, profile);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "ConfigHandler:InitAsync:Error;");
			_output.Write("Error occured while initializing configuration profile", LogLevel.Error);
		}
	}
}
