using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Repository.Models;
using Dingo.Core.Services.Adapters;

namespace Dingo.Core.Repository.Mapper;

internal sealed class DbModelMapper : IDbModelMapper
{
	private readonly IPath _path;

	public DbModelMapper(IPath path)
	{
		_path = path.Required(nameof(path));
	}

	public MigrationComparisonInput ToMigrationsInfoInput(Migration migration)
	{
		return new MigrationComparisonInput(migration.Hash.Value, migration.Path.Relative);
	}

	public PatchMigration ToPathMigration(DbPatchMigration patchMigration)
	{
		return new PatchMigration(
			patchMigration.MigrationHash,
			new MigrationPath(
				string.Empty,
				patchMigration.MigrationPath,
				_path.GetRootDirectory(patchMigration.MigrationPath),
				_path.GetFileName(patchMigration.MigrationPath)
			),
			patchMigration.PatchNumber
		);
	}
}
