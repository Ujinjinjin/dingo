using LinqToDB;

namespace Dingo.Core.Constants;

/// <summary> Database provider constants </summary>
public static class DbProvider
{
	/// <summary> List of currently supported database providers </summary>
	public static readonly IList<string> SupportedDatabaseProviderNames = new[]
	{
		ProviderName.PostgreSQL95,
		ProviderName.SqlServer2017,
	};
}