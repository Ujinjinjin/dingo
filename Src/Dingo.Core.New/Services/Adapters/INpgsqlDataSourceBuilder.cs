using Microsoft.Extensions.Logging;
using Npgsql;
using Npgsql.TypeMapping;

namespace Dingo.Core.Services.Adapters;

public interface INpgsqlDataSourceBuilder
{
	/// <inheritdoc cref="NpgsqlDataSourceBuilder.UseLoggerFactory"/>
	public INpgsqlDataSourceBuilder UseLoggerFactory(ILoggerFactory? loggerFactory);

	/// <inheritdoc cref="NpgsqlDataSourceBuilder.MapComposite{T}"/>
	public INpgsqlTypeMapper MapComposite<T>(string? pgName = null, INpgsqlNameTranslator? nameTranslator = null);

	/// <inheritdoc cref="NpgsqlDataSourceBuilder.Build"/>
	public INpgsqlDataSource Build();
}
