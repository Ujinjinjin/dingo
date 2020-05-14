using Dingo.Cli.DbUtils;
using Dingo.Cli.Loggers;
using LinqToDB;
using Microsoft.Extensions.Logging;

namespace Dingo.Cli.Repository
{
	internal sealed class PostgresDbContext : DataConnectionBase
	{
		private static readonly ILogger Logger = new ConsoleLogger();
		
		protected internal PostgresDbContext(string connectionString)
			: base(ProviderName.SqlServer2017, connectionString, Logger)
		{
		}
	}
}
