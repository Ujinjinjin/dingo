using Dingo.Abstractions;
using Dingo.Abstractions.Config;
using Dingo.Abstractions.Operations;
using System;
using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	public class ConfigOperations : BaseOperations, IConfigOperations
	{
		public ConfigOperations(
			IGlobalConfig globalConfig, 
			IProjectConfig projectConfig
		) : base(globalConfig, projectConfig)
		{
		}
		
		public async Task ConfigDatabaseAsync(bool global, string connectionString, DatabaseEngine? dbEngine)
		{
			var (updatable, config) = GetConfig(global);

			config.ConnectionString = connectionString ?? config.ConnectionString;
			config.DatabaseEngine = dbEngine ?? config.DatabaseEngine;

			await updatable.UpdateAsync();
		}

		public void ShowConfigs(bool global)
		{
			var (_, config) = GetConfig(global);
			
			Console.WriteLine("Database configs:");
			Console.WriteLine($"\tConnection string: {config.ConnectionString}");
			Console.WriteLine($"\tDB provider: {config.DatabaseEngine.ToString()}");
		}
	}
}
