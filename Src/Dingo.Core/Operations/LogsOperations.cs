using Dingo.Core.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	/// <inheritdoc />
	internal class LogsOperations : ILogsOperations
	{
		private readonly IPathHelper _pathHelper;

		public LogsOperations(IPathHelper pathHelper)
		{
			_pathHelper = pathHelper ?? throw new ArgumentNullException(nameof(pathHelper));
		}

		/// <inheritdoc />
		public Task PruneLogsAsync()
		{
			var directoryInfo = new DirectoryInfo(_pathHelper.GetLogsDirectory());

			foreach (var file in directoryInfo.EnumerateFiles())
			{
				file.Delete();
			}

			foreach (var dir in directoryInfo.EnumerateDirectories())
			{
				dir.Delete(true);
			}

			return Task.CompletedTask;
		}
	}
}
