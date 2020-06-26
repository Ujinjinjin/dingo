using LinqToDB;
using System.Collections.Generic;

namespace Dingo.Core.Constants
{
	public static class DbProvider
	{
		public static readonly IList<string> SupportedDatabaseProviderNames = new[]
		{
			ProviderName.PostgreSQL95
		};
	}
}
