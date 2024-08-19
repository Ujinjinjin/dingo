namespace Dingo.Core.Services.Handlers;

public interface IMigrationHandler
{
	/// <summary> Create new migration file with specified name </summary>
	Task CreateAsync(string name, string path, CancellationToken ct = default);

	/// <summary> Show migrations status </summary>
	Task ShowStatusAsync(string? profile, string path, CancellationToken ct = default);

	/// <summary> Apply database migrations </summary>
	Task MigrateAsync(string? profile, string path, CancellationToken ct = default);

	/// <summary> Rollback last N patches </summary>
	Task RollbackAsync(string? profile, string path, int? patchCount, bool force, CancellationToken ct = default);
}
