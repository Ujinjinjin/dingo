using Dingo.Core.Models;
using Dingo.Core.Repository.Models;

namespace Dingo.Core.Repository.Mapper;

public interface IDbModelMapper
{
	MigrationComparisonInput ToMigrationsInfoInput(Migration migration);
	PatchMigration ToPathMigration(DbPatchMigration patchMigration);
}
