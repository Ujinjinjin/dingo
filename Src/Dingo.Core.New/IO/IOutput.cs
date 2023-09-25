using Microsoft.Extensions.Logging;

namespace Dingo.Core.IO;

internal interface IOutput
{
	public void Write(string message, LogLevel level);
}
