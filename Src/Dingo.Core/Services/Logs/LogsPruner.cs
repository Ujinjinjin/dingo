using Dingo.Core.Extensions;
using Dingo.Core.Services.Adapters;

namespace Dingo.Core.Services.Logs;

internal class LogsPruner : ILogsPruner
{
	private readonly IPath _path;
	private readonly IFile _file;
	private readonly IDirectory _directory;

	public LogsPruner(
		IPath path,
		IFile file,
		IDirectory directory
	)
	{
		_path = path.Required(nameof(path));
		_file = file.Required(nameof(file));
		_directory = directory.Required(nameof(directory));
	}

	public void Prune()
	{
		var logsPath = _path.GetLogsPath();
		if (!_directory.Exists(logsPath))
		{
			return;
		}

		var logFiles = _directory.GetFiles(logsPath, "*", SearchOption.AllDirectories);
		foreach (var logFile in logFiles)
		{
			_file.Delete(logFile);
		}
	}
}
