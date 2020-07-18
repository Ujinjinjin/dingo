using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.Facades
{
	/// <summary> Facade over <see cref="System.IO.File"/> for unit testing purposes </summary>
	public interface IFileFacade
	{
		/// <inheritdoc cref="File.Exists"/>
		bool Exists(string path);
		
		/// <inheritdoc cref="File.ReadAllTextAsync(string,System.Threading.CancellationToken)"/>
		Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);
		
		/// <inheritdoc cref="File.ReadAllTextAsync(string,System.Text.Encoding,System.Threading.CancellationToken)"/>
		Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);
	}
}
