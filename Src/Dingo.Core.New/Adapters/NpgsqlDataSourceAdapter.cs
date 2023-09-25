using System.Data;
using System.Data.Common;
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

	public DbConnection CreateConnection()
	{
		return _dataSource.CreateConnection();
	}
}
