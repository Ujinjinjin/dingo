namespace Dingo.Core.Models;

public record MigrationPath(string Absolute, string Relative, string Module, string Filename)
{
	public string Absolute { get; set; } = Absolute;
	public string Relative { get; set; } = Relative;
	public string Module { get; set; } = Module;
	public string Filename { get; set; } = Filename;
}
