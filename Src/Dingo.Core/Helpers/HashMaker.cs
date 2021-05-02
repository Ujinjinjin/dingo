using Dingo.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Dingo.Core.Helpers
{
	/// <inheritdoc />
	internal class HashMaker : IHashMaker
	{
		/// <inheritdoc />
		public async Task<string> GetFileHashAsync(string filename)
		{
			using var sha512 = SHA512.Create();
			await using var stream = File.OpenRead(filename);

			var hash = await sha512.ComputeHashAsync(stream);

			return BitConverter.ToString(hash)
				.Replace("-", "")
				.ToLowerInvariant();
		}

		/// <inheritdoc />
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
	}
}
