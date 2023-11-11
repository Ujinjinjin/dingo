using Dingo.Core.Models;
using Dingo.Core.Repository.Models;

namespace Dingo.Core.Services.Migrations;

public interface IMigrationStatusCalculator
{
	bool NeedToApplyMigration(Migration migration);

	PatchMigrationStatus CalculatePatchMigrationStatus(
		PatchMigration patchMigration,
		Dictionary<string, Migration> localMigrationsMap
	);
}
