namespace Dingo.Core.Models;

public enum MigrationType
{
	None = 0,

	/// <summary> System migrations </summary>
	System = 1,

	/// <summary> User defined migrations </summary>
	User = 2,
}
