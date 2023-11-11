namespace Dingo.Core.Models;

[Flags]
public enum PatchMigrationStatus
{
	/// <summary> Unknown status </summary>
	Unknown = 0,

	/// <summary> Everything is OK, patch can be rolled back </summary>
	Ok = 1 << 0,

	/// <summary> There are some minor issues, patch can be rolled back forcefully </summary>
	Warning = 1 << 1,

	/// <summary> Previously applied migration not found locally. Might've been deleted </summary>
	LocalMigrationNotFound = Warning | 1 << 2,

	/// <summary> Local migration was modified since last patch </summary>
	LocalMigrationModified = Warning | 1 << 3,
}
