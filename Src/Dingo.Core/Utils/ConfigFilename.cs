namespace Dingo.Core.Utils;

internal static class ConfigFilename
{
	public static string Build(string? profile)
	{
		return string.IsNullOrEmpty(profile)
			? $"{Constants.ConfigFilename}.{Constants.ConfigExtension}"
			: $"{Constants.ConfigFilename}.{profile}.{Constants.ConfigExtension}";
	}
}
