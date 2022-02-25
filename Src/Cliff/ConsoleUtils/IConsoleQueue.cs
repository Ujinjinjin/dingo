namespace Cliff.ConsoleUtils;

/// <summary> Console output queue </summary>
public interface IConsoleQueue
{
	/// <summary> Enqueue line of symbols with specified length </summary>
	/// <param name="length">Line length</param>
	/// <param name="symbol">Symbols from which line is built</param>
	/// <param name="newLineBefore">Insert new line symbol before</param>
	/// <param name="newLineAfter">Insert new line symbol after</param>
	Task EnqueueBreakLine(int? length = null, char symbol = '-', bool newLineBefore = true, bool newLineAfter = true);

	/// <summary> Enqueue output string </summary>
	/// <param name="value">String to enqueue</param>
	public Task EnqueueOutputAsync(string value);
}