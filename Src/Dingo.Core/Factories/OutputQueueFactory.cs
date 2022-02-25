using Dingo.Core.Adapters;
using Dingo.Core.IO;

namespace Dingo.Core.Factories;

/// <inheritdoc />
internal sealed class OutputQueueFactory : IOutputQueueFactory
{
	private readonly IDirectoryAdapter _directoryAdapter;
	private readonly IFileAdapter _fileAdapter;

	public OutputQueueFactory(IDirectoryAdapter directoryAdapter, IFileAdapter fileAdapter)
	{
		_directoryAdapter = directoryAdapter ?? throw new ArgumentNullException(nameof(directoryAdapter));
		_fileAdapter = fileAdapter ?? throw new ArgumentNullException(nameof(fileAdapter));
	}

	/// <inheritdoc />
	public IOutputQueue CreateFileOutputQueue() => new FileOutputQueue(_directoryAdapter, _fileAdapter);

	/// <inheritdoc />
	public IOutputQueue CreateConsoleOutputQueue() => new ConsoleOutputQueue();
}