using System.Threading.Tasks;

namespace Dingo.Core.Services;

/// <summary> Logs operations </summary>
public interface ILogsService
{
	/// <summary> Switch level of logging </summary>
	/// <param name="configPath">Custom path to configuration file</param>
	/// <param name="logLevel">Level of logging</param>
	Task SwitchLogLevelAsync(string configPath = null, int? logLevel = null);

	/// <summary> Prune log files </summary>
	Task PruneLogsAsync();
}