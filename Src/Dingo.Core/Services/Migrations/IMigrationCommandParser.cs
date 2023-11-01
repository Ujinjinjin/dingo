using Dingo.Core.Models;

namespace Dingo.Core.Services.Migrations;

public interface IMigrationCommandParser
{
	MigrationCommand Parse(string sql);
}
