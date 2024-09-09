using Dingo.Core.Models;
using Dingo.Core.Repository.Models;

namespace Dingo.Core.Services.Migrations;

internal sealed class MigrationStatusCalculator : IMigrationStatusCalculator
{
	public bool NeedToApplyMigration(Migration migration)
	{
		return migration.Status is MigrationStatus.New or MigrationStatus.Outdated or MigrationStatus.ForceOutdated;
	}

	public PatchMigrationStatus CalculatePatchMigrationStatus(
		PatchMigration patchMigration,
		Dictionary<string, Migration> localMigrationsMap
	)
	{
		var status = PatchMigrationStatus.None;

		if (!localMigrationsMap.TryGetValue(patchMigration.MigrationPath, out var localMigration))
		{
			status |= PatchMigrationStatus.LocalMigrationNotFound;
			return status;
		}

		if (localMigration.Status != MigrationStatus.UpToDate)
		{
			status |= PatchMigrationStatus.LocalMigrationModified;
		}

		if (patchMigration.MigrationHash != localMigration.Hash.Value)
		{
			status |= PatchMigrationStatus.LocalMigrationModified;
		}

		if (status == PatchMigrationStatus.None)
		{
			status |= PatchMigrationStatus.Ok;
		}

		return status;
	}
}
