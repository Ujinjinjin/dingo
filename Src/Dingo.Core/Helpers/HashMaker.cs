using Dingo.Core.Models;
using System.Security.Cryptography;
using System.Text;

namespace Dingo.Core.Helpers;

/// <inheritdoc />
internal sealed class HashMaker : IHashMaker
{
	/// <inheritdoc />
	public async Task<string> GetFileHashAsync(string filename)
	{
		using var sha512 = SHA512.Create();
		var byteContent = Encoding.UTF8.GetBytes(await File.ReadAllTextAsync(filename)); 

		var hash = sha512.ComputeHash(byteContent);

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