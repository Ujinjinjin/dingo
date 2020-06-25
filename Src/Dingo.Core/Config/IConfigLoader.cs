using Dingo.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.Config
{
	/// <summary> Loads project configurations </summary>
	public interface IConfigLoader
	{
		/// <summary> Load project configurations </summary>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Result of loading with configuration itself and file from which they are loaded</returns>
		Task<LoadConfigResult> LoadProjectConfigAsync(CancellationToken cancellationToken = default);

		/// <summary> Load project configurations </summary>
		/// <param name="configPath">Custom path to configuration file</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Result of loading with configuration itself and file from which they are loaded</returns>
		Task<LoadConfigResult> LoadProjectConfigAsync(string configPath, CancellationToken cancellationToken = default);
	}
}