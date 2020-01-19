using System.Threading.Tasks;

namespace Dingo.Abstractions.Config
{
	public interface ILoadable
	{
		/// <summary> Load configurations from file </summary>
		Task LoadAsync();
	}
}
