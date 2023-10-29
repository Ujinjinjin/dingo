using Dingo.Core.Services.Adapters;

namespace Dingo.Core.Repository.Source;

public interface INpgsqlDataSourceProvider
{
	INpgsqlDataSource Instance();
}
