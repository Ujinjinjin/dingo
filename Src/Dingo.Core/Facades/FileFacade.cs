using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.Facades
{
	/// <inheritdoc />
	public class FileFacade : IFileFacade
	{
		
		/// <inheritdoc />
		public bool Exists(string path)
		{
			return File.Exists(path);
		}
		
		/// <inheritdoc />
		public async Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
		{
			return await File.ReadAllTextAsync(path, cancellationToken);
		}

		/// <inheritdoc />
		public async Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
		{
			return await File.ReadAllTextAsync(path, encoding, cancellationToken);
		}
	}
}
