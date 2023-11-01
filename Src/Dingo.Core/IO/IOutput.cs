using Microsoft.Extensions.Logging;

namespace Dingo.Core.IO;

public interface IOutput
{
	public void Write(string message, LogLevel level);
}
