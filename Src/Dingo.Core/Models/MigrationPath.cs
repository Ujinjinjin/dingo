namespace Dingo.Core.Models;

public sealed record MigrationPath(string Absolute, string Relative, string Module, string Filename);
