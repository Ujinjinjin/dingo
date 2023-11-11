namespace Dingo.Core.Models;

public record MigrationCommand(string? Up, string? Down)
{
	public string Up { get; } = Up;
	public string? Down { get; } = Down;

	public static MigrationCommand Empty => new(default, default);
}
