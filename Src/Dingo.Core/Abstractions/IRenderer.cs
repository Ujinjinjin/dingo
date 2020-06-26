using Dingo.Core.Config;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Core.Abstractions
{
	/// <summary> Abstraction over displaying information to user </summary>
	public interface IRenderer
	{
		/// <summary> Show project configurations to user </summary>
		/// <param name="configWrapper">Configuration wrapper</param>
		Task ShowConfigAsync(IConfigWrapper configWrapper);

		/// <summary> Show non-interactable message to user </summary>
		/// <param name="message">Message shown to user</param>
		Task ShowMessageAsync(string message);

		/// <summary> Display list of items </summary>
		/// <param name="itemList">Items to be listed</param>
		Task ListItemsAsync(IList<string> itemList);
	}
}