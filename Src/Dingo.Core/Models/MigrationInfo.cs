namespace Dingo.Core.Models
{
	/// <summary> Info about migration file </summary>
	public struct MigrationInfo
	{
		/// <summary> Path to migration file </summary>
		public FilePath Path { get; set; }
		
		/// <summary> Migration file hash </summary>
		public string NewHash { get; set; }
		
		/// <summary> File's old migration hash (if it was applied earlier) </summary>
		public string OldHash { get; set; }
		
		/// <summary> Migration status </summary>
		public MigrationStatus Status { get; set; }
	}
}