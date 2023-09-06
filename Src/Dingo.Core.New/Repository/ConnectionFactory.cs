using System.Data;
using Dingo.Core.Extensions;
using Microsoft.Data.SqlClient;
using Npgsql;
using Trico.Configuration;

namespace Dingo.Core.Repository;

internal class ConnectionFactory : IConnectionFactory
{
	private readonly IConfiguration _configuration;

	public ConnectionFactory(IConfiguration configuration)
	{
		_configuration = configuration.Required(nameof(configuration));
	}

	public IDbConnection Create()
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
			_ when provider.Equals(ProviderName.SqlServer, StringComparison.InvariantCultureIgnoreCase) => new SqlConnection(connectionString),
			_ when provider.Equals(ProviderName.PostgreSql, StringComparison.InvariantCultureIgnoreCase) => new NpgsqlConnection(connectionString),
			_ when provider.Equals(ProviderName.Postgres, StringComparison.InvariantCultureIgnoreCase) => new NpgsqlConnection(connectionString),
			_ => throw new ArgumentOutOfRangeException(
				nameof(provider),
				$"Database provider {provider} is not supported"
			),
		};
	}
}
