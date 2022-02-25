using Cliff.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.CommandLine;

namespace Cliff.Infrastructure;

/// <inheritdoc />
public sealed class CliService : ICliService
{
	/// <inheritdoc />
	public IServiceProvider ServiceProvider { get; }

	private readonly ILogger _logger;

	public CliService(IServiceProvider serviceProvider)
	{
		ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

		_logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger<CliService>() ?? throw new ArgumentNullException(nameof(ILoggerFactory));
	}

	/// <inheritdoc />
	public async Task ExecuteAsync(string[] args)
	{
		try
		{
			var rootCommand = ServiceProvider
				.RegisterControllers()
				.GetService<RootCommand>();

			if (rootCommand is null)
			{
				throw new Exception($"Couldn't find any registered {nameof(RootCommand)}");
			}

			await rootCommand.InvokeAsync(args);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error occured during command execution with args: {args}");
			Console.WriteLine("Error! Please try again or contact the maintainer of this solution");
		}
	}
}