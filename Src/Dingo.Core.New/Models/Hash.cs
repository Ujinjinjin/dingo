using System.Security.Cryptography;
using System.Text;
using Dingo.Core.Adapters;

namespace Dingo.Core.Models;

public record Hash(string Value)
{
	public string Value { get; private set; } = Value;

	public static async Task<Hash> ComputeAsync(IFileAdapter fileAdapter, MigrationPath migrationPath)
	{
		using var sha512 = SHA512.Create();
		var byteContent = Encoding.UTF8.GetBytes(await fileAdapter.ReadAllTextAsync(migrationPath.Absolute));

		var rawHash = sha512.ComputeHash(byteContent);

		var strHash = BitConverter.ToString(rawHash)
			.Replace("-", "")
			.ToLowerInvariant();

		return new Hash(strHash);
	}
}
