namespace Dingo.Core.Models
{
	/// <summary> Info about migration file </summary>
	internal struct MigrationInfo
	{
		/// <summary> Path to migration file </summary>
		public FilePath Path { get; set; }
		
		/// <summary> Migration file hash </summary>
		public string NewHash { get; set; }
		
		/// <summary> File's old migration hash (if it was applied earlier) </summary>
		public string OldHash { get; set; }
		
		/// <summary> Action required to apply to migration </summary>
		public MigrationAction Action { get; set; }
	}
}