﻿using Dingo.Core.Repository.DbClasses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Core.Repository
{
	internal interface IDatabaseContext: IDisposable
	{
		/// <summary> Check if table exists in specified schema </summary>
		/// <param name="schema">Database schema</param>
		/// <param name="table">Database table</param>
		Task<DbSystemCheckTableExistenceResult> CheckTableExistenceAsync(string schema, string table);
		
		/// <summary> Execute raw sql query </summary>
		/// <param name="sql">Raw sql query</param>
		Task ExecuteRawSqlAsync(string sql);
		
		/// <summary> Register migration in database </summary>
		/// <param name="migrationPath">Path to migration file</param>
		/// <param name="migrationHash">Hash of migration file</param>
		/// <param name="dateUpdated">Timestamp when migration was updated in database</param>
		Task RegisterMigrationAsync(string migrationPath, string migrationHash, DateTime dateUpdated);
		
		/// <summary> Get status of migration list </summary>
		/// <param name="dbMigrationInfoInputList">List of migrations</param>
		Task<IList<DbMigrationInfoOutput>> GetMigrationsStatusAsync(IList<DbMigrationInfoInput> dbMigrationInfoInputList);
	}
}