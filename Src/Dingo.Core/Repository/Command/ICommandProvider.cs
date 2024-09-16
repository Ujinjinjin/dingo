using Dingo.Core.Models;
using Dingo.Core.Repository.Models;

namespace Dingo.Core.Repository.Command;

public interface ICommandProvider
{
	Command SelectSchema(string schema);
	Command GetMigrationsStatus(IReadOnlyList<MigrationComparisonInput> migrationInfoInputs);
	Command GetNextPatch(PatchType patchType);
	Command GetLastPatchMigrations(int patchCount);
	Command RegisterMigration(Migration migration, int patchNumber);
	Command RevertPatch(int patchNumber);
	Command CompletePatch(int patchNumber);
}
