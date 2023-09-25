using System.Data;
using System.Data.Common;

namespace Dingo.Core.Adapters;

public interface INpgsqlDataSource
{
	DbConnection CreateConnection();
}
