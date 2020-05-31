namespace Dingo.Cli.Models
{
	internal struct MigrationInfo
	{
		public FilePath Path { get; set; }
		public string Hash { get; set; }
	}
}