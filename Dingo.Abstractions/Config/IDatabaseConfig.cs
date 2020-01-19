namespace Dingo.Abstractions.Config
{
	public interface IDatabaseConfig
	{
		string ConnectionString { get; set; }
		DatabaseEngine? DatabaseEngine { get; set; }
	}
}
