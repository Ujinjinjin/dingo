namespace Dingo.Core.Models;

internal interface IMigrationMeta
{
	MigrationPath Path { get; }
	Hash Hash { get; }
}
