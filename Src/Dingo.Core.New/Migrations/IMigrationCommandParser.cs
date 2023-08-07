using Dingo.Core.Models;

namespace Dingo.Core.Migrations;

public interface IMigrationCommandParser
{
	MigrationCommand Parse(string sql);
}
