namespace Dingo.Core.Migrations;

public interface IMigrationRunner
{
	Task MigrateAsync(string path, CancellationToken ct = default);
	Task RollbackAsync(string path, int patchCount, bool force, CancellationToken ct = default);
}
