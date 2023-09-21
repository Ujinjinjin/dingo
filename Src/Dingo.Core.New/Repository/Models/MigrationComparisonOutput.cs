namespace Dingo.Core.Repository.Models;

public record MigrationComparisonOutput
{
	public string Hash { get; set; }
	public bool? HashMatches { get; set; }
}
