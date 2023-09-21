using System.Data;

namespace Dingo.Core.Adapters;

public interface INpgsqlDataSource
{
	IDbConnection CreateConnection();
}
