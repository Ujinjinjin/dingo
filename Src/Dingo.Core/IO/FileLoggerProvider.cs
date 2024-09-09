using Dingo.Core.Extensions;
using Dingo.Core.Services.Adapters;
using Microsoft.Extensions.Logging;
using Trico.Configuration;

namespace Dingo.Core.IO;

internal sealed class FileLoggerProvider : ILoggerProvider
{
	private readonly IConfiguration _configuration;
	private readonly IFile _file;
	private readonly IDirectory _directory;
	private readonly IPath _path;

	public FileLoggerProvider(
		IConfiguration configuration,
		IFile file,
		IDirectory directory,
		IPath path
	)
	{
		_configuration = configuration.Required(nameof(configuration));
		_file = file.Required(nameof(file));
		_directory = directory.Required(nameof(directory));
		_path = path.Required(nameof(path));
	}

	public void Dispose()
	{
	}

	public ILogger CreateLogger(string categoryName)
	{
		return new FileLogger(categoryName, _configuration, _file, _directory, _path);
	}
}
