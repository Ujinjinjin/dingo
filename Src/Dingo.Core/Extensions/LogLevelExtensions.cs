using Microsoft.Extensions.Logging;

namespace Dingo.Core.Extensions
{
	/// <summary> Collection of extensions for <see cref="LogLevel"/> </summary>
	internal static class LogLevelExtensions
	{
		public static string ToString(this LogLevel logLevel)
		{
			return logLevel switch
			{
				LogLevel.Trace => "Trace",
				LogLevel.Debug => "Debug",
				LogLevel.Information => "Information",
				LogLevel.Warning => "Warning",
				LogLevel.Error => "Error",
				LogLevel.Critical => "Critical",
				LogLevel.None => "None",
				_ => null
			};
		}
	}
}
