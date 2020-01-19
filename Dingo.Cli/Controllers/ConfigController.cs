using Dingo.Abstractions;
using Dingo.Abstractions.Infrastructure;
using Dingo.Abstractions.Operations;
using Dingo.Core.Attributes;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace Dingo.Cli.Controllers
{
	[SubCommand("config", "Configure dingo")]
	internal class ConfigController : IController
	{
		private readonly IConfigOperations _configOperations;
		
		public ConfigController(IConfigOperations configOperations)
		{
			_configOperations = configOperations ?? throw new ArgumentNullException(nameof(configOperations));
		}
		
		[SubCommand("database", "Configure project database settings")]
		[Option("Global configuration", typeof(bool), false, "--global", "-g")]
		[Option("Connection string", typeof(string), false, "--connectionString", "-cs")]
		[Option("Database engine", typeof(DatabaseEngine), false, "--dbEngine", "-de")]
		[UsedImplicitly]
		public async Task ConfigureDatabase(bool global, string connectionString, DatabaseEngine? dbEngine)
		{
			await _configOperations.ConfigDatabaseAsync(global, connectionString, dbEngine);
		}

		[SubCommand("show", "Show all dingo configurations")]
		[Option("Global configuration", typeof(bool), false, "--global", "-g")]
		[UsedImplicitly]
		public void ShowConfigurations(bool global)
		{
			_configOperations.ShowConfigs(global);
		}
	}
}
