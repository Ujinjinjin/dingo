using Dingo.Core.Adapters;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dingo.Core.IO
{
	/// <summary> Queue with output to file. Inherited from <see cref="IOutputQueue"/> </summary>
	internal sealed class FileOutputQueue : OutputQueueBase
	{
		private readonly IDirectoryAdapter _directoryAdapter;
		private readonly IFileAdapter _fileAdapter;

		public FileOutputQueue(IDirectoryAdapter directoryAdapter, IFileAdapter fileAdapter)
		{
			_directoryAdapter = directoryAdapter ?? throw new ArgumentNullException(nameof(directoryAdapter));
			_fileAdapter = fileAdapter ?? throw new ArgumentNullException(nameof(fileAdapter));
		}

		/// <inheritdoc />
		protected override async Task QueueWorkerAsync()
		{
			while (true)
			{
				if (TryDequeue(out var queueItem))
				{
					var outputDirectory = Path.GetDirectoryName(queueItem.OutputPath);
					if (!_directoryAdapter.Exists(outputDirectory))
					{
						var _ = _directoryAdapter.CreateDirectory(outputDirectory);
					}
					
					if (!_fileAdapter.Exists(queueItem.OutputPath))
					{
						_fileAdapter.Create(queueItem.OutputPath).Close();
					}

					await using var writer = _fileAdapter.AppendText(queueItem.OutputPath);
					await writer.WriteLineAsync(queueItem.OutputValue);
				}
			}
			// ReSharper disable once FunctionNeverReturns
		}
	}
}
