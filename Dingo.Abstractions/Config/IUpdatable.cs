using System.Threading.Tasks;

namespace Dingo.Abstractions.Config
{
	public interface IUpdatable
	{
		/// <summary> Update config files </summary>
		Task UpdateAsync();
	}
}
