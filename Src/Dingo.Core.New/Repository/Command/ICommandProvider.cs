using Dingo.Core.Repository.Models;

namespace Dingo.Core.Repository.Command;

public interface ICommandProvider
{
	Command SelectSchema(string schema);
	Command GetMigrationsStatus(IReadOnlyList<MigrationComparisonInput> migrationInfoInputs);
}
