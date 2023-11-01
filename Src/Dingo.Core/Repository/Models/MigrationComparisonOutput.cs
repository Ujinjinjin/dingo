namespace Dingo.Core.Repository.Models;

public record MigrationComparisonOutput
{
	public string MigrationHash { get; set; }
	public bool? HashMatches { get; set; }
}
