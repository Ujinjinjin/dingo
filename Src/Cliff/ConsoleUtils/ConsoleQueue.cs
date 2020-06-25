using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Cliff.ConsoleUtils
{
	internal class ConsoleQueue : IConsoleQueue
	{
		private readonly BlockingCollection<string> _outputQueue = new BlockingCollection<string>();

		public ConsoleQueue()
		{
			var thread = new Thread(
				() =>
				{
					while (true)
					{
						Console.WriteLine(_outputQueue.Take());
					}
				});
			thread.IsBackground = true;
			thread.Start();
		}
		
		public Task EnqueueOutputAsync(string value)
		{
			return Task.FromResult(_outputQueue.TryAdd(value));
		}
	}
}
