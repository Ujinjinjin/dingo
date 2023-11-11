using Dingo.Core.Models;
using Dingo.Core.Repository.Models;

namespace Dingo.Core.Repository;

public interface IRepository
{
	Task<bool> TryHandshakeAsync(CancellationToken ct = default);
	Task<bool> SchemaExistsAsync(string schema, CancellationToken ct = default);
	Task<bool> IsDatabaseEmptyAsync(CancellationToken ct = default);

	Task<IReadOnlyList<MigrationComparisonOutput>> GetMigrationsComparisonAsync(
		IReadOnlyList<Migration> migrations,
		CancellationToken ct = default
	);

	Task<int> GetNextPatchAsync(CancellationToken ct = default);
	Task<IReadOnlyList<PatchMigration>> GetLastPatchMigrationsAsync(int patchCount, CancellationToken ct = default);
	Task RegisterMigrationAsync(Migration migration, int patchNumber, CancellationToken ct = default);
	Task RevertPatchAsync(int patchNumber, CancellationToken ct = default);
	Task CompletePatchAsync(int patchNumber, CancellationToken ct = default);

	Task ExecuteAsync(string sql, CancellationToken ct = default);
	Task ReloadTypesAsync(CancellationToken ct = default);
}
