using Dingo.Core.Adapters;

namespace Dingo.Core.Repository.Source;

public interface INpgsqlDataSourceProvider
{
	INpgsqlDataSource Instance();
}
