namespace Dingo.Cli.Models
{
	internal struct MigrationInfo
	{
		public FilePath Path { get; set; }
		public string NewHash { get; set; }
		public string OldHash { get; set; }
		public MigrationAction Action { get; set; }
	}
}