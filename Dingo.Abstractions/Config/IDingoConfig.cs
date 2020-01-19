namespace Dingo.Abstractions.Config
{
	public interface IDingoConfig : IDatabaseConfig
	{
		/// <summary> Directory of dingo artifacats and configuration files </summary>
		string DingoDirectory { get; }
	}
}
