using Dingo.Core.Adapters;
using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;
using Dingo.Core.Repository.Models;
using Microsoft.Extensions.Logging;
using Trico.Configuration;

namespace Dingo.Core.Repository.Source;

internal class NpgsqlDataSourceProvider : INpgsqlDataSourceProvider
{
	private readonly IConfiguration _configuration;
	private readonly INpgsqlDataSourceBuilder _dataSourceBuilder;
	private readonly ILoggerFactory _loggerFactory;

	private INpgsqlDataSource? _npgsqlDataSource;

	public NpgsqlDataSourceProvider(
		IConfiguration configuration,
		INpgsqlDataSourceBuilder dataSourceBuilder,
		ILoggerFactory loggerFactory
	)
	{
		_configuration = configuration.Required(nameof(configuration));
		_dataSourceBuilder = dataSourceBuilder.Required(nameof(dataSourceBuilder));
		_loggerFactory = loggerFactory.Required(nameof(loggerFactory));
	}

	public INpgsqlDataSource Instance()
	{
		return _npgsqlDataSource ??= BuildSource();
	}

	private INpgsqlDataSource BuildSource()
	{
		var provider = _configuration.Get(Configuration.Key.DatabaseProvider);

		if (!provider.IsPostgres())
		{
			throw new IncompatibleDatabaseException(provider);
		}

		_dataSourceBuilder
			.UseLoggerFactory(_loggerFactory)
			.MapComposite<MigrationComparisonInput>("dingo.t_migration_info_input");

		return _dataSourceBuilder.Build();
	}
}
