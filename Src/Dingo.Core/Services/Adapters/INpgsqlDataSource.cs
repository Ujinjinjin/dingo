using System.Data.Common;

namespace Dingo.Core.Services.Adapters;

public interface INpgsqlDataSource
{
	DbConnection CreateConnection();
}
