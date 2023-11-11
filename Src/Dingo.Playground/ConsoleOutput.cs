using Dingo.Core.IO;
using Microsoft.Extensions.Logging;

namespace Dingo.Playground;

public class ConsoleOutput : IOutput
{
	public void Write(string message, LogLevel level)
	{
		Console.WriteLine($"[{level}] {message}");
	}
}
