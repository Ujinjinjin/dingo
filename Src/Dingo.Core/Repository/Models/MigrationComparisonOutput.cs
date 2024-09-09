namespace Dingo.Core.Repository.Models;

public sealed record MigrationComparisonOutput
{
	public string MigrationHash { get; set; }
	public bool? HashMatches { get; set; }
}
