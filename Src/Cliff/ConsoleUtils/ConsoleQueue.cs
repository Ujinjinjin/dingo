using System.Collections.Concurrent;
using System.Text;

namespace Cliff.ConsoleUtils;

/// <inheritdoc cref="Cliff.ConsoleUtils.IConsoleQueue" />
internal sealed class ConsoleQueue : IConsoleQueue, IDisposable
{
	private readonly ConcurrentQueue<string> _outputQueue = new();
	private readonly Thread _thread;

	public ConsoleQueue()
	{
		// ReSharper disable once UseObjectOrCollectionInitializer
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
		length ??= Console.WindowWidth - 1;
		var stringBuilder = new StringBuilder();

		if (newLineBefore)
		{
			stringBuilder.Append('\n');
		}

		stringBuilder.Append(new string(symbol, length.Value));

		if (newLineAfter)
		{
			stringBuilder.Append('\n');
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