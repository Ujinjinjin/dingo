namespace Dingo.Core.Models;

public enum MigrationType
{
	Unknown = 0,

	/// <summary> System migrations </summary>
	System = 1,

	/// <summary> User defined migrations </summary>
	User = 2,
}
