using System.Data;
using Microsoft.Extensions.Logging;

namespace Dingo.Core.Repository;

internal class DatabaseRepository : IRepository
{
	private readonly IConnectionFactory _connectionFactory;
	private readonly ILogger _logger;


	public DatabaseRepository(IConnectionFactory connectionFactory, ILoggerFactory loggerFactory)
	{
		_connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
		_logger = loggerFactory?.CreateLogger<DatabaseRepository>() ?? throw new ArgumentNullException(nameof(loggerFactory));
	}

	public bool TryHandshake()
	{
		try
		{
			Handshake();
			return true;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Can't establish database connection");
			return false;
		}
	}

	private void Handshake()
	{
		using var connection = _connectionFactory.Create();

		if (connection.State == ConnectionState.Open)
		{
			return;
		}

		connection.Open();
	}
}
