using Dingo.Core.Models;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.IO
{
	/// <summary> Queue with output to file. Inherited from <see cref="IOutputQueue"/> </summary>
	internal class FileOutputQueue : IOutputQueue, IDisposable
	{
		private readonly ConcurrentQueue<OutputQueueItem> _outputQueue = new ConcurrentQueue<OutputQueueItem>();
		private readonly Thread _thread;

		public FileOutputQueue()
		{
			_thread = new Thread(async () => await QueueWorker());
			_thread.IsBackground = true;
			_thread.Start();
		}

		private async Task QueueWorker()
		{
			while (true)
			{
				if (_outputQueue.TryDequeue(out var queueItem))
				{
					await using var writer = File.AppendText(queueItem.OutputPath);
					await writer.WriteLineAsync(queueItem.OutputValue);
				}
			}
			// ReSharper disable once FunctionNeverReturns
		}

		/// <inheritdoc />
		public void EnqueueOutput(string outputPath, string outputValue)
		{
			_outputQueue.Enqueue(new OutputQueueItem {OutputPath = outputValue, OutputValue = outputValue});
		}

		/// <inheritdoc />
		public void Dispose()
		{
			_outputQueue?.Clear();
			_thread.Interrupt();
		}
	}
}
