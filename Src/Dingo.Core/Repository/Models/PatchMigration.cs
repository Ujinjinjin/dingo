namespace Dingo.Core.Repository.Models;

public sealed class PatchMigration
{
	public string MigrationHash { get; set; }
	public string MigrationPath { get; set; }
	public int PatchNumber { get; set; }
}
