using Dingo.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Dingo.Core.Helpers
{
	/// <inheritdoc />
	internal class HashMaker : IHashMaker
	{
		/// <inheritdoc />
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

		/// <inheritdoc />
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
