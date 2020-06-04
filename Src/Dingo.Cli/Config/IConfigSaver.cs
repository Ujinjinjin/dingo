using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Cli.Config
{
	public interface IConfigSaver
	{
		Task SaveProjectConfigAsync(IConfiguration configuration, CancellationToken cancellationToken = default);
		Task SaveProjectConfigAsync(IConfiguration configuration, string configPath, CancellationToken cancellationToken = default);
	}
}