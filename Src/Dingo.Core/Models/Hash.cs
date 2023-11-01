using System.Security.Cryptography;
using System.Text;
using Dingo.Core.Services.Adapters;

namespace Dingo.Core.Models;

public record Hash(string Value)
{
	public string Value { get; private set; } = Value;

	public static async Task<Hash> ComputeAsync(IFile file, MigrationPath migrationPath)
	{
		using var sha = SHA256.Create();
		var byteContent = Encoding.UTF8.GetBytes(await file.ReadAllTextAsync(migrationPath.Absolute));

		var rawHash = sha.ComputeHash(byteContent);

		var strHash = BitConverter.ToString(rawHash)
			.Replace("-", "")
			.ToLowerInvariant();

		return new Hash(strHash);
	}
}
