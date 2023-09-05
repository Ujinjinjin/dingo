namespace Dingo.Core.Migrations;

public interface IMigrationGenerator
{
	Task GenerateAsync(string name, string path);
}
