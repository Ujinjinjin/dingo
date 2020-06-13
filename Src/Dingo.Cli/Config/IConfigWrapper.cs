using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Cli.Config
{
	public interface IConfigWrapper : IConfiguration
	{
		Task SaveAsync(CancellationToken cancellationToken = default);
		Task SaveAsync(string configPath, CancellationToken cancellationToken = default);
		Task LoadAsync(CancellationToken cancellationToken = default);
		Task LoadAsync(string configPath, CancellationToken cancellationToken = default);
	}
}