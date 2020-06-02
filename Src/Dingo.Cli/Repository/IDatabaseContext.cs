﻿using Dingo.Cli.Repository.DbClasses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dingo.Cli.Repository
{
	internal interface IDatabaseContext: IDisposable
	{
		Task<DbSystemCheckTableExistenceResult> CheckTableExistenceAsync(string schema, string table);
		Task ExecuteRawSqlAsync(string sql);
		Task RegisterMigrationAsync(string migrationPath, string migrationHash, DateTime dateUpdated);
		Task<IList<DbMigrationInfoOutput>> GetMigrationsStatusAsync(IList<DbMigrationInfoInput> dbMigrationInfoInputList);
	}
}