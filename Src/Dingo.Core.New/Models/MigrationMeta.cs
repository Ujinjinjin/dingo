namespace Dingo.Core.Models;

internal sealed class MigrationMeta : IMigrationMeta
{
	public MigrationPath Path { get; }
	public Hash Hash { get; }

	public MigrationMeta(MigrationPath migrationPath, Hash hash)
	{
		Path = migrationPath;
		Hash = hash;
	}
}
