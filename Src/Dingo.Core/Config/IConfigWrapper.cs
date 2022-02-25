using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.Config;

/// <summary> Wrapper over default and project configurations </summary>
public interface IConfigWrapper : IConfiguration
{
	/// <summary> Path to the file from which configurations were loaded </summary>
	string ActiveConfigFile { get; }

	/// <summary> Flag of existance of specified configuration file </summary>
	bool ConfigFileExists { get; }

	/// <summary> Save current project configurations to file </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	Task SaveAsync(CancellationToken cancellationToken = default);

	/// <summary> Save current project configurations to file </summary>
	/// <param name="configPath">Custom path to configuration file</param>
	/// <param name="cancellationToken">Cancellation token</param>
	Task SaveAsync(string configPath, CancellationToken cancellationToken = default);

	/// <summary> Load  project configurations </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	Task LoadAsync(CancellationToken cancellationToken = default);

	/// <summary> Load  project configurations </summary>
	/// <param name="configPath">Custom path to configuration file</param>
	/// <param name="cancellationToken">Cancellation token</param>
	Task LoadAsync(string configPath, CancellationToken cancellationToken = default);
}