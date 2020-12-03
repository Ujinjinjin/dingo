using System.IO;
using System.Threading.Tasks;

namespace Dingo.Core.IO
{
	/// <summary> Queue with output to file. Inherited from <see cref="IOutputQueue"/> </summary>
	internal class FileOutputQueue : OutputQueueBase
	{
		protected override async Task QueueWorkerAsync()
		{
			while (true)
			{
				if (TryDequeue(out var queueItem))
				{
					await using var writer = File.AppendText(queueItem.OutputPath);
					await writer.WriteLineAsync(queueItem.OutputValue);
				}
			}
			// ReSharper disable once FunctionNeverReturns
		}
	}
}
