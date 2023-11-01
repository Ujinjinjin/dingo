using Trico.Configuration;

namespace Dingo.Core.Extensions;

internal static class ConfigurationExtensions
{
	private const string True = "true";
	private const string False = "false";

	public static bool IsTrue(this IConfiguration configuration, string key)
	{
		var value = configuration.Get(key);
		return string.Equals(value, True, StringComparison.OrdinalIgnoreCase);
	}

	public static bool IsFalse(this IConfiguration configuration, string key)
	{
		var value = configuration.Get(key);
		return string.Equals(value, False, StringComparison.OrdinalIgnoreCase);
	}
}
