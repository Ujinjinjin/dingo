using Dingo.Cli.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Dingo.Cli.Operations
{
	internal class HashMaker : IHashMaker
	{
		public Task<string> GetFileHashAsync(string filename)
		{
			return Task.FromResult(GetFileHash(filename));
		}

		public string GetFileHash(string filename)
		{
			using (var md5 = MD5.Create())
			{
				using (var stream = File.OpenRead(filename))
				{
					var hash = md5.ComputeHash(stream);
					
					return BitConverter.ToString(hash)
						.Replace("-", "")
						.ToLowerInvariant();
				}
			}
		}
		
		public async Task<IList<MigrationInfo>> GetMigrationInfoListAsync(IList<FilePath> filePathList)
		{
			var migrationInfoList = new MigrationInfo[filePathList.Count];
			for (var i = 0; i < filePathList.Count; i++)
			{
				migrationInfoList[i] = new MigrationInfo
				{
					Path = filePathList[i],
					NewHash = await GetFileHashAsync(filePathList[i].Absolute)
				};
			}

			return migrationInfoList;
		}
		
		public IList<MigrationInfo> GetMigrationInfoList(IList<FilePath> filePathList)
		{
			var migrationInfoList = new MigrationInfo[filePathList.Count];
			for (var i = 0; i < filePathList.Count; i++)
			{
				migrationInfoList[i] = new MigrationInfo
				{
					Path = filePathList[i],
					NewHash = GetFileHash(filePathList[i].Absolute)
				};
			}

			return migrationInfoList;
		}
	}
}
