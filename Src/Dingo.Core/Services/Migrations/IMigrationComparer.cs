using Dingo.Core.Models;

namespace Dingo.Core.Services.Migrations;

public interface IMigrationComparer
{
	Task<IReadOnlyList<Migration>> CalculateMigrationsStatusAsync(
		IReadOnlyList<Migration> migrations,
		CancellationToken ct = default
	);
}
