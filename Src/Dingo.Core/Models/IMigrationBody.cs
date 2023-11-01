namespace Dingo.Core.Models;

internal interface IMigrationBody
{
	MigrationCommand Command { get; }
	MigrationStatus Status { get; set; }
}
