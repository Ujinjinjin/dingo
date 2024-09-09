using Dingo.Core.Extensions;
using Microsoft.Extensions.Logging;
using Npgsql;
using Npgsql.TypeMapping;
using Trico.Configuration;

namespace Dingo.Core.Services.Adapters;

internal sealed class NpgsqlDataSourceBuilderAdapter : INpgsqlDataSourceBuilder
{
	private readonly IConfiguration _configuration;

	private NpgsqlDataSourceBuilder? _builder;
	private NpgsqlDataSourceBuilder Builder
	{
		get
		{
			if (_builder is not null) return _builder;

			var connectionString = _configuration.Get(Configuration.Key.ConnectionString);
			_builder = new NpgsqlDataSourceBuilder(connectionString);

			return _builder;
		}
	}

	public NpgsqlDataSourceBuilderAdapter(IConfiguration configuration)
	{
		_configuration = configuration.Required(nameof(configuration));
	}

	public INpgsqlDataSourceBuilder UseLoggerFactory(ILoggerFactory? loggerFactory)
	{
		Builder.UseLoggerFactory(loggerFactory);
		return this;
	}

	public INpgsqlTypeMapper MapComposite<T>(string? pgName = null, INpgsqlNameTranslator? nameTranslator = null)
	{
		return Builder.MapComposite<T>(pgName, nameTranslator);
	}

	public INpgsqlDataSource Build()
	{
		return new NpgsqlDataSourceAdapter(Builder.Build());
	}
}
