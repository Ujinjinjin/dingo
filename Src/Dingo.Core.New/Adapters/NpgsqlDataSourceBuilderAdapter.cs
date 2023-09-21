using Microsoft.Extensions.Logging;
using Npgsql;
using Npgsql.TypeMapping;
using Trico.Configuration;

namespace Dingo.Core.Adapters;

public class NpgsqlDataSourceBuilderAdapter : INpgsqlDataSourceBuilder
{
	private readonly NpgsqlDataSourceBuilder _builder;

	public NpgsqlDataSourceBuilderAdapter(IConfiguration configuration)
	{
		var connectionString = configuration.Get(Configuration.Key.ConnectionString);
		_builder = new NpgsqlDataSourceBuilder(connectionString);
	}

	public INpgsqlDataSourceBuilder UseLoggerFactory(ILoggerFactory? loggerFactory)
	{
		_builder.UseLoggerFactory(loggerFactory);
		return this;
	}

	public INpgsqlTypeMapper MapComposite<T>(string? pgName = null, INpgsqlNameTranslator? nameTranslator = null)
	{
		return _builder.MapComposite<T>(pgName, nameTranslator);
	}

	public INpgsqlDataSource Build()
	{
		return new NpgsqlDataSourceAdapter(_builder.Build());
	}
}
