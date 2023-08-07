using System.Text;

namespace Dingo.Core.Adapters;

/// <summary> Adapter for <see cref="System.IO.File"/> for unit testing purposes </summary>
public interface IFileAdapter
{
	/// <inheritdoc cref="File.AppendText"/>
	StreamWriter AppendText(string path);

	/// <inheritdoc cref="File.Create(string)"/>
	FileStream Create(string path);

	/// <inheritdoc cref="File.Exists"/>
	bool Exists(string path);

	/// <inheritdoc cref="File.ReadAllTextAsync(string,System.Threading.CancellationToken)"/>
	Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);

	/// <inheritdoc cref="File.ReadAllTextAsync(string,System.Text.Encoding,System.Threading.CancellationToken)"/>
	Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);
}