using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.Config
{
	/// <summary> Saves project configurations </summary>
	public interface IConfigSaver
	{
		/// <summary> Save configurations to file </summary>
		/// <param name="configuration">Configuration model to be saved</param>
		/// <param name="cancellationToken">Cancellation token</param>
		Task SaveProjectConfigAsync(IConfiguration configuration, CancellationToken cancellationToken = default);

		/// <summary> Save configurations to file </summary>
		/// <param name="configuration">Configuration model to be saved</param>
		/// <param name="configPath">Custom path to configuration file</param>
		/// <param name="cancellationToken">Cancellation token</param>
		Task SaveProjectConfigAsync(IConfiguration configuration, string configPath, CancellationToken cancellationToken = default);
	}
}