using Dingo.Core.Abstractions;
using Sharprompt;

namespace Dingo.Cli.Implementors
{
	/// <summary> CLI interactions with user </summary>
	public class CliPrompt : IPrompt
	{
		/// <inheritdoc />
		public bool Confirm(string message, bool? defaultValue = null)
		{
			return Prompt.Confirm(message, defaultValue);
		}
	}
}
