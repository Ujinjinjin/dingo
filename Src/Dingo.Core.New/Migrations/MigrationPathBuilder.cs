using Dingo.Core.Adapters;
using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;
using Dingo.Core.Repository;
using Trico.Configuration;

namespace Dingo.Core.Migrations;

internal class MigrationPathBuilder : IMigrationPathBuilder
{
	private readonly IPathAdapter _pathAdapter;
	private readonly IConfiguration _configuration;

	public MigrationPathBuilder(
		IPathAdapter pathAdapter,
		IConfiguration configuration
	)
	{
		_pathAdapter = pathAdapter.Required(nameof(pathAdapter));
		_configuration = configuration.Required(nameof(configuration));
	}

	public string BuildSystemMigrationsPath()
	{
		var providerName = _configuration.Get(Configuration.Key.DatabaseProvider);
		var providerDir = providerName switch
		{
			ProviderName.SqlServer => ProviderName.SqlServer,
			ProviderName.Postgres => ProviderName.PostgreSql,
			ProviderName.PostgreSql => ProviderName.PostgreSql,
			_ => throw new DatabaseProviderNotSupportedException(providerName),
		};

		return _pathAdapter.Join(
			_pathAdapter.GetApplicationPath(),
			"Scripts",
			providerDir
		);
	}
}
