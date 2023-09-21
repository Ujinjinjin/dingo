using Dingo.Core.Models;
using Dingo.Core.Repository.Models;

namespace Dingo.Core.Repository;

public interface IRepository
{
	bool TryHandshake();
	Task<bool> SchemaExistsAsync(string schema, CancellationToken ct = default);
	Task<IReadOnlyList<MigrationComparisonOutput>> GetMigrationsComparisonAsync(
		IReadOnlyList<Migration> migrations,
		CancellationToken ct = default
	);
}
