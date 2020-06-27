namespace Dingo.Core.Models
{
	internal enum MigrationAction
	{
		/// <summary> Unknown action </summary>
		Unknown = 0,

		/// <summary> Migration is not aplied yet </summary>
		Install = 1,

		/// <summary> Older version of migration has been aplied earlier and can be updated </summary>
		Update = 2,

		/// <summary> Migration is up to date, so no actions are required </summary>
		Skip = 3,
	}
}