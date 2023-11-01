using System.Drawing;
using Dingo.Core.IO;
using Microsoft.Extensions.Logging;
using Pastel;

namespace Dingo.Cli.Implementors;

public sealed class CliOutput : IOutput
{
	public void Write(string message, LogLevel level)
	{
		var textColor = GetMessageColor(level);
		message = message.Pastel(textColor);

		var prefix = GetMessagePrefix(level);
		if (!string.IsNullOrWhiteSpace(prefix))
		{
			message = prefix + message;
		}

		Console.WriteLine(message);
	}

	private Color GetMessageColor(LogLevel level)
	{
		return level switch
		{
			LogLevel.Trace => Color.DarkGray,
			LogLevel.Debug => Color.DarkGray,
			LogLevel.Information => Color.LightGray,
			LogLevel.Warning => Color.Peru,
			LogLevel.Error => Color.Tomato,
			LogLevel.Critical => Color.Tomato,
			LogLevel.None => Color.Empty,
			_ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
		};
	}

	private string GetMessagePrefix(LogLevel level)
	{
		return level switch
		{
			LogLevel.Trace => string.Empty,
			LogLevel.Debug => string.Empty,
			LogLevel.Information => string.Empty,
			LogLevel.Warning => string.Empty,
			LogLevel.Error => string.Empty,
			LogLevel.Critical => "[CRITICAL]. ",
			LogLevel.None => string.Empty,
			_ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
		};
	}
}
