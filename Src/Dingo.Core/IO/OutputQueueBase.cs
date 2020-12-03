using Dingo.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Dingo.Core.IO
{
	/// <summary> Base output queue. Inherited from <see cref="IOutputQueue"/> </summary>
	internal abstract class OutputQueueBase : IOutputQueue, IDisposable
	{
		private readonly ConcurrentQueue<OutputQueueItem> _outputQueue = new ConcurrentQueue<OutputQueueItem>();
		private readonly Thread _thread;

		protected OutputQueueBase()
		{
			_thread = new Thread(async () => await QueueWorkerAsync());
			_thread.IsBackground = true;
			_thread.Start();
		}

		/// <summary> Worker to process queue </summary>
		protected abstract Task QueueWorkerAsync();

		/// <summary> Try to dequeue item </summary>
		/// <param name="queueItem">Queue item</param>
		/// <returns>Result of dequeuing an item from queue</returns>
		protected bool TryDequeue(out OutputQueueItem queueItem) => _outputQueue.TryDequeue(out queueItem);

		/// <inheritdoc />
		public void EnqueueOutput(string outputValue, string outputPath)
		{
			_outputQueue.Enqueue(new OutputQueueItem {OutputValue = outputValue, OutputPath = outputPath});
		}

		/// <inheritdoc />
		public void Dispose()
		{
			_outputQueue?.Clear();
			_thread.Interrupt();
		}
	}
}
