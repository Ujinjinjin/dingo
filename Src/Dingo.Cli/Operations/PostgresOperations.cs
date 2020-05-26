using Dingo.Cli.Repository;
using System;
using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal class PostgresOperations : IDbOperations
	{
		private readonly IPathHelper _pathHelper;
		private readonly IConfiguration _configuration;

		public PostgresOperations(IPathHelper pathHelper, IConfiguration configuration)
		{
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task<bool> CheckSystemTableExistence()
		{
			await using (var dbContext = new PostgresDbContext(_configuration.ConnectionString))
			{
				await dbContext.DummyAsync();
			}

			return true;
		}

		public async Task InstallDingoProcedures()
		{
			await using (var dbContext = new PostgresDbContext(_configuration.ConnectionString))
			{
				
			}
		}
	}
}
