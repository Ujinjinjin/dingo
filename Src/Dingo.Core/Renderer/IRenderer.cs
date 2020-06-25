using Dingo.Core.Config;
using System.Threading.Tasks;

namespace Dingo.Core.Renderer
{
	public interface IRenderer
	{
		Task ShowConfigAsync(IConfigWrapper configuration);

		Task ShowMessage(string message);
	}
}