﻿using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	/// <summary> Operations with project configurations </summary>
	public interface IConfigOperations
	{
		/// <summary> Initialize dingo config file </summary>
		/// <param name="configPath">Custom path to configuration file</param>
		Task InitConfigurationFileAsync(string configPath = null);

		/// <summary> Show current configurations </summary>
		/// <param name="configPath">Custom path to configuration file</param>
		Task ShowProjectConfigurationAsync(string configPath = null);

		/// <summary> Update specified configuration </summary>
		/// <param name="configPath">Custom path to configuration file</param>
		/// <param name="connectionString">Database connection string</param>
		/// <param name="providerName">Database provider name</param>
		/// <param name="migrationSchema">Database schema for you migrations</param>
		/// <param name="migrationTable">Database table, where all migrations are stored</param>
		/// <param name="searchPattern">Pattern to search migration files in specified directory</param>
		Task UpdateProjectConfigurationAsync(
			string configPath = null,
			string connectionString = null,
			string providerName = null,
			string migrationSchema = null,
			string migrationTable = null,
			string searchPattern = null
		);
	}
}
