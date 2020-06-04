using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Cli.Config
{
	public interface IConfigLoader
	{
		Task<IConfiguration> LoadProjectConfigAsync(CancellationToken cancellationToken = default);
		Task<IConfiguration> LoadProjectConfigAsync(string configPath, CancellationToken cancellationToken = default);
	}
}