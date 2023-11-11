using Dingo.Core.Models;

namespace Dingo.Core.Services.Migrations;

public interface IMigrationApplier
{
	Task ApplyAndRegisterAsync(Migration migration, int patchNumber, CancellationToken ct = default);
	Task ApplyAsync(Migration migration, CancellationToken ct = default);
	Task RegisterAsync(Migration migration, int patchNumber, CancellationToken ct = default);
	Task RevertAsync(Migration migration, CancellationToken ct = default);
}
