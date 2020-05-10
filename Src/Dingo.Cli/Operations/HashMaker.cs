using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		
		public Task<IDictionary<string, string>> GetFileListHashAsync(IList<string> filenameList)
		{
			return Task.FromResult(GetFileListHash(filenameList));
		}
		
		public IDictionary<string, string> GetFileListHash(IList<string> filenameList)
		{
			return filenameList.ToDictionary(x => x, GetFileHash);
		}
	}
}
