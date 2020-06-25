using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.Config
{
	public interface IConfigWrapper : IConfiguration
	{
		string ActiveConfigFile { get; set; }
		Task SaveAsync(CancellationToken cancellationToken = default);
		Task SaveAsync(string configPath, CancellationToken cancellationToken = default);
		Task LoadAsync(CancellationToken cancellationToken = default);
		Task LoadAsync(string configPath, CancellationToken cancellationToken = default);
	}
}