namespace Dingo.Core.Services.Migrations;

public interface IMigrationGenerator
{
	Task GenerateAsync(string name, string path, CancellationToken ct = default);
}
