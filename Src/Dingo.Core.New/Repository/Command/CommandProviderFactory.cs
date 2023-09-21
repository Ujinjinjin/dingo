using Dingo.Core.Extensions;
using Trico.Configuration;

namespace Dingo.Core.Repository.Command;

public class CommandProviderFactory : ICommandProviderFactory
{
	private readonly IConfiguration _configuration;

	public CommandProviderFactory(IConfiguration configuration)
	{
		_configuration = configuration.Required(nameof(configuration));
	}

	public ICommandProvider Create()
	{
		var provider = _configuration.Get(Configuration.Key.DatabaseProvider);

		if (string.IsNullOrWhiteSpace(provider))
		{
			throw new ArgumentNullException(nameof(provider), "Provider must be specified");
		}

		return provider switch
		{
			_ when provider.Equals(ProviderName.SqlServer, StringComparison.InvariantCultureIgnoreCase) => new SqlServerCommandProvider(),
			_ when provider.Equals(ProviderName.PostgreSql, StringComparison.InvariantCultureIgnoreCase) => new PostgreSqlCommandProvider(),
			_ when provider.Equals(ProviderName.Postgres, StringComparison.InvariantCultureIgnoreCase) => new PostgreSqlCommandProvider(),
			_ => throw new ArgumentOutOfRangeException(
				nameof(provider),
				$"Database provider {provider} is not supported"
			),
		};
	}
}
