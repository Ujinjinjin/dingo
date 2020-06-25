using Dingo.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.Config
{
	public interface IConfigLoader
	{
		Task<LoadConfigResult> LoadProjectConfigAsync(CancellationToken cancellationToken = default);
		Task<LoadConfigResult> LoadProjectConfigAsync(string configPath, CancellationToken cancellationToken = default);
	}
}