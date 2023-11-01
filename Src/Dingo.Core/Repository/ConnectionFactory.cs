using System.Data;
using System.Data.Common;
using Dingo.Core.Extensions;
using Dingo.Core.Repository.Source;
using Microsoft.Data.SqlClient;
using Trico.Configuration;

namespace Dingo.Core.Repository;

internal class ConnectionFactory : IConnectionFactory
{
	private readonly IConfiguration _configuration;
	private readonly INpgsqlDataSourceProvider _npgsqlProvider;

	public ConnectionFactory(IConfiguration configuration, INpgsqlDataSourceProvider npgsqlProvider)
	{
		_configuration = configuration.Required(nameof(configuration));
		_npgsqlProvider = npgsqlProvider.Required(nameof(npgsqlProvider));
	}

	public DbConnection Create()
	{
		var provider = _configuration.Get(Configuration.Key.DatabaseProvider);
		var connectionString = _configuration.Get(Configuration.Key.ConnectionString);

		if (string.IsNullOrWhiteSpace(provider))
		{
			throw new ArgumentNullException(nameof(provider), "Provider must be specified");
		}

		if (string.IsNullOrWhiteSpace(connectionString))
		{
			throw new ArgumentNullException(nameof(connectionString), "Connection string must be provided");
		}

		return provider switch
		{
			_ when provider.IsSqlServer() => new SqlConnection(connectionString),
			_ when provider.IsPostgres() => _npgsqlProvider.Instance().CreateConnection(),
			_ => throw new ArgumentOutOfRangeException(
				nameof(provider),
				$"Database provider {provider} is not supported"
			),
		};
	}
}
