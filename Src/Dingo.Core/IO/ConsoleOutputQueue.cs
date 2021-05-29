using System;
using System.Threading.Tasks;

namespace Dingo.Core.IO
{
	/// <summary> Queue with output to console. Inherited from <see cref="IOutputQueue"/> </summary>
	internal sealed class ConsoleOutputQueue : OutputQueueBase
	{
		protected override Task QueueWorkerAsync()
		{
			while (true)
			{
				if (TryDequeue(out var queueItem))
				{
					Console.WriteLine(queueItem.OutputValue);
				}
			}
			// ReSharper disable once FunctionNeverReturns
		}
	}
}
