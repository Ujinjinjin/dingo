namespace Dingo.Core.Repository.Models;

public class PatchMigration
{
	public string MigrationHash { get; set; }
	public string MigrationPath { get; set; }
	public int PatchNumber { get; set; }
}
