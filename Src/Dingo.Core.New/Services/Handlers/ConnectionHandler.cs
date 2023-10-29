using Dingo.Core.Extensions;
using Dingo.Core.IO;
using Dingo.Core.Repository;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Services.Handlers;

internal class ConnectionHandler : IConnectionHandler
{
	private readonly IRepository _repository;
	private readonly IOutput _output;
	private readonly ILogger _logger;

	public ConnectionHandler(
		IRepository repository,
		IOutput output,
		ILoggerFactory loggerFactory
	)
	{
		_repository = repository.Required(nameof(repository));
		_output = output.Required(nameof(output));
		_logger = loggerFactory.Required(nameof(loggerFactory))
			.CreateLogger<MigrationHandler>()
			.Required(nameof(loggerFactory));
	}

	public async Task HandshakeAsync(CancellationToken ct = default)
	{
		try
		{
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
