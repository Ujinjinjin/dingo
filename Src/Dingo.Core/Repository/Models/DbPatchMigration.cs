namespace Dingo.Core.Repository.Models;

public sealed record DbPatchMigration
{
	public string MigrationHash { get; set; }
	public string MigrationPath { get; set; }
	public int PatchNumber { get; set; }
}
