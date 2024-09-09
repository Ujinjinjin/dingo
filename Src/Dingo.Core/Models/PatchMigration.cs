namespace Dingo.Core.Models;

public sealed record PatchMigration(string MigrationHash, MigrationPath MigrationPath, int PatchNumber);
