using System;
using System.IO;
using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal class ProgramOperations : IProgramOperations
	{
		private readonly IConfiguration _configuration;
		private readonly IDatabaseOperations _databaseOperations;
		private readonly IDirectoryScanner _directoryScanner;
		private readonly IHashMaker _hashMaker;
		private readonly IPathHelper _pathHelper;

		public ProgramOperations(
			IConfiguration configuration,
			IDatabaseOperations databaseOperations,
			IDirectoryScanner directoryScanner,
			IHashMaker hashMaker,
			IPathHelper pathHelper
		)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_databaseOperations = databaseOperations ?? throw new ArgumentNullException(nameof(databaseOperations));
			_directoryScanner = directoryScanner ?? throw new ArgumentNullException(nameof(directoryScanner));
			_hashMaker = hashMaker ?? throw new ArgumentNullException(nameof(hashMaker));
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		}

		public async Task RunAsync(string[] args)
		{
			await _databaseOperations.InstallCheckTableExistenceProcedureAsync();

			var migrationTableExists = await _databaseOperations.CheckMigrationTableExistenceAsync();

			var migrationsRootPath = _pathHelper.GetApplicationBasePath() + _configuration.DingoMigrationsRootPath;
			var filePathList = await _directoryScanner.GetFilePathListAsync(migrationsRootPath, _configuration.MigrationsSearchPattern);

			var migrationInfoList = await _hashMaker.GetMigrationInfoListAsync(filePathList);

			if (!migrationTableExists)
			{
				for (var i = 0; i < migrationInfoList.Count; i++)
				{
					var sqlScriptText = await File.ReadAllTextAsync(migrationInfoList[i].Path.Absolute);
					await _databaseOperations.ApplyMigrationAsync(sqlScriptText, migrationInfoList[i].Path.Relative, migrationInfoList[i].Hash, true);
				}
				
				for (var i = 0; i < migrationInfoList.Count; i++)
				{
					await _databaseOperations.RegisterMigrationAsync(migrationInfoList[i].Path.Relative, migrationInfoList[i].Hash);
				}
			}
			else
			{
				// for (var i = 0; i < migrationInfoList.Count; i++)
				// {
				// 	var sqlScriptText = await File.ReadAllTextAsync(migrationInfoList[i].Path.Absolute);
				// 	await _databaseOperations.ApplyMigrationAsync(sqlScriptText, migrationInfoList[i].Path.Relative, migrationInfoList[i].Hash, true);
				// }
			}
		}
	}
}