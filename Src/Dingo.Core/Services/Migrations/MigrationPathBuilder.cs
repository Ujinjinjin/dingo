using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;
using Dingo.Core.Repository;
using Dingo.Core.Services.Adapters;
using Trico.Configuration;

namespace Dingo.Core.Services.Migrations;

internal sealed class MigrationPathBuilder : IMigrationPathBuilder
{
	private readonly IPath _path;
	private readonly IConfiguration _configuration;

	public MigrationPathBuilder(
		IPath path,
		IConfiguration configuration
	)
	{
		_path = path.Required(nameof(path));
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

		return _path.Join(
			_path.GetAppDataPath(),
			"Scripts",
			providerDir
		);
	}
}
