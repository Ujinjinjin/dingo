namespace Dingo.Core.Models;

/// <summary> Migration status </summary>
public enum MigrationStatus
{
	/// <summary> Unknown action </summary>
	Unknown = 0,

	/// <summary> Migration is not applied yet </summary>
	New = 1,

	/// <summary> Applied earlier migration has been outdated and newer version can be installed </summary>
	Outdated = 2,

	/// <summary> Migration is up to date, so no actions are required </summary>
	UpToDate = 3,
}