using Dingo.Core.Extensions;
using Dingo.Core.Services.Adapters;
using Microsoft.Extensions.Logging;
using Trico.Configuration;

namespace Dingo.Core.IO;

internal class FileLoggerProvider : ILoggerProvider
{
	private readonly IConfiguration _configuration;
	private readonly IFileAdapter _fileAdapter;
	private readonly IDirectoryAdapter _directoryAdapter;
	private readonly IPathAdapter _pathAdapter;

	public FileLoggerProvider(
		IConfiguration configuration,
		IFileAdapter fileAdapter,
		IDirectoryAdapter directoryAdapter,
		IPathAdapter pathAdapter
	)
	{
		_configuration = configuration.Required(nameof(configuration));
		_fileAdapter = fileAdapter.Required(nameof(fileAdapter));
		_directoryAdapter = directoryAdapter.Required(nameof(directoryAdapter));
		_pathAdapter = pathAdapter.Required(nameof(pathAdapter));
	}

	public void Dispose()
	{
	}

	public ILogger CreateLogger(string categoryName)
	{
		return new FileLogger(categoryName, _configuration, _fileAdapter, _directoryAdapter, _pathAdapter);
	}
}
