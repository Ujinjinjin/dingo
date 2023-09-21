using System.Data;
using Dingo.Core.Extensions;
using Npgsql;

namespace Dingo.Core.Adapters;

public class NpgsqlDataSourceAdapter : INpgsqlDataSource
{
	private readonly NpgsqlDataSource _dataSource;

	public NpgsqlDataSourceAdapter(NpgsqlDataSource dataSource)
	{
		_dataSource = dataSource.Required(nameof(dataSource));
	}

	public IDbConnection CreateConnection()
	{
		return _dataSource.CreateConnection();
	}
}
