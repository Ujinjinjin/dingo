using System.Text;

namespace Dingo.Core.Adapters;

/// <inheritdoc />
public sealed class FileAdapter : IFileAdapter
{
	/// <inheritdoc />
	public StreamWriter AppendText(string path) => File.AppendText(path);

	/// <inheritdoc />
	public FileStream Create(string path) => File.Create(path);

	/// <inheritdoc />
	public bool Exists(string path) => File.Exists(path);

	/// <inheritdoc />
	public async Task<string> ReadAllTextAsync(
		string path,
		CancellationToken ct = default
	) => await File.ReadAllTextAsync(path, ct);

	/// <inheritdoc />
	public async Task<string> ReadAllTextAsync(
		string path,
		Encoding encoding,
		CancellationToken ct = default
	) => await File.ReadAllTextAsync(path, encoding, ct);
}
