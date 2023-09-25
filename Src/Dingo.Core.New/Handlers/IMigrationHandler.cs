namespace Dingo.Core.Handlers;

public interface IMigrationHandler
{
	/// <summary> Create new migration file with specified name </summary>
	Task CreateAsync(string name, string path, CancellationToken ct = default);

	/// <summary> Show migrations status </summary>
	Task ShowStatusAsync(string path, CancellationToken ct = default);

	/// <summary> Apply database migrations </summary>
	Task MigrateAsync(string path, CancellationToken ct = default);
}
