using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cliff.ConsoleUtils
{
	/// <inheritdoc cref="Cliff.ConsoleUtils.IConsoleQueue" />
	internal class ConsoleQueue : IConsoleQueue, IDisposable
	{
		private readonly ConcurrentQueue<string> _outputQueue = new ConcurrentQueue<string>();
		private readonly Thread _thread;

		public ConsoleQueue()
		{
			_thread = new Thread(
				() =>
				{
					while (true)
					{
						if (_outputQueue.TryDequeue(out var value))
						{
							Console.WriteLine(value);	
						}
					}
					// ReSharper disable once FunctionNeverReturns
				});
			_thread.IsBackground = true;
			_thread.Start();
		}

		/// <inheritdoc />
		public async Task EnqueueBreakLine(int? length = null, char symbol = '-', bool newLineBefore = true, bool newLineAfter = true)
		{
			length ??= Console.WindowWidth;
			var stringBuilder = new StringBuilder();

			if (newLineBefore)
			{
				stringBuilder.Append("\n");
			}

			stringBuilder.Append(new string(symbol, length.Value));
			
			if (newLineAfter)
			{
				stringBuilder.Append("\n");
			}
			
			await EnqueueOutputAsync(stringBuilder.ToString());
		}

		/// <inheritdoc />
		public Task EnqueueOutputAsync(string value)
		{
			_outputQueue.Enqueue(value);
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			_outputQueue?.Clear();
			_thread.Interrupt();
		}
	}
}
