﻿using Dingo.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Core.Helpers
{
	/// <summary> Database helper </summary>
	internal interface IDatabaseHelper
	{
		/// <summary> Check if dingo migrations table exists in database </summary>
		/// <returns>Check result</returns>
		Task<bool> CheckMigrationTableExistenceAsync();

		/// <summary> Install database stored procedure that allows to check table existence by name </summary>
		Task InstallCheckTableExistenceProcedureAsync();

		/// <summary> Apply migration </summary>
		/// <param name="sql">Sql text of migration</param>
		/// <param name="migrationPath">Path to migration file</param>
		/// <param name="migrationHash">MD5 hash of migration file</param>
		/// <param name="silent">If true, migration will be applied, but not registred in migrations table</param>
		Task ApplyMigrationAsync(string sql, string migrationPath, string migrationHash, bool silent = false);
		
		/// <summary> Register migration without applying it </summary>
		/// <param name="migrationPath">Path to migration file</param>
		/// <param name="migrationHash">MD5 hash of migration file</param>
		Task RegisterMigrationAsync(string migrationPath, string migrationHash);

		/// <summary> Get status of given migrations </summary>
		/// <param name="migrationInfoList">List of migrations to check</param>
		/// <returns>List of migrations with actual status</returns>
		Task<IList<MigrationInfo>> GetMigrationsStatusAsync(IList<MigrationInfo> migrationInfoList);
	}
}