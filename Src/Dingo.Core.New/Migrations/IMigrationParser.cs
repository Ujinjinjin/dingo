namespace Dingo.Core.Migrations;

public interface IMigrationParser
{
	Migration Parse(string sql);
}
