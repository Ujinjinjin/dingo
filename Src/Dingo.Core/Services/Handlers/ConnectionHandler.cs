using Dingo.Core.Extensions;
using Dingo.Core.IO;
using Dingo.Core.Repository;
using Dingo.Core.Services.Config;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Services.Handlers;

internal class ConnectionHandler : IConnectionHandler
{
	private readonly IConfigProfileLoader _profileLoader;
	private readonly IRepository _repository;
	private readonly IOutput _output;
	private readonly ILogger _logger;

	public ConnectionHandler(
		IConfigProfileLoader profileLoader,
		IRepository repository,
		IOutput output,
		ILoggerFactory loggerFactory
	)
	{
		_profileLoader = profileLoader.Required(nameof(profileLoader));
		_repository = repository.Required(nameof(repository));
		_output = output.Required(nameof(output));
		_logger = loggerFactory.Required(nameof(loggerFactory))
			.CreateLogger<ConnectionHandler>()
			.Required(nameof(loggerFactory));
	}

	public async Task HandshakeAsync(string? profile, CancellationToken ct = default)
	{
		try
		{
			await _profileLoader.LoadAsync(profile, ct);
			await _HandshakeAsync(ct);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "ConnectionHandler:HandshakeAsync:Error;");
			_output.Write($"Error occured while applying migrations: {ex.Message}", LogLevel.Error);
		}
	}

	private async Task _HandshakeAsync(CancellationToken ct = default)
	{
		if (await _repository.TryHandshakeAsync(ct))
		{
			_output.Write("Database connection successfully established", LogLevel.Information);
			return;
		}

		_output.Write("Can't establish database connection, check logs for more info", LogLevel.Error);
	}
}
