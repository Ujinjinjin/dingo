namespace Dingo.Core.Models;

public enum MigrationStatus
{
	/// <summary> Unknown action </summary>
	Unknown = 0,

	/// <summary> Migration is up to date, so no actions are required </summary>
	UpToDate = 1,

	/// <summary> Migration is not applied yet </summary>
	New = 2,

	/// <summary> Applied earlier migration has been outdated and newer version can be installed </summary>
	Outdated = 3,

	/// <summary> Migration is in Force Directory and wil be applied regardless of its status </summary>
	ForceOutdated = 4,
}
