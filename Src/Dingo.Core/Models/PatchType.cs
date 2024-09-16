namespace Dingo.Core.Models;

public enum PatchType
{
	None = 0,

	/// <summary> System migrations </summary>
	System = 1,

	/// <summary> User defined migrations </summary>
	User = 2,
}
