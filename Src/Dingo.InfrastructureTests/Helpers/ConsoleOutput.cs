namespace Dingo.InfrastructureTests.Helpers;

public class ConsoleOutput : IDisposable
{
	private readonly StringWriter _output;
	private readonly TextWriter _originalOutput;

	public ConsoleOutput()
	{
		_output = new StringWriter();
		_originalOutput = Console.Out;
		Console.SetOut(_output);
	}

	public string Get()
	{
		var output = _output.ToString();

		return string.IsNullOrEmpty(output)
			? output
			: _output.ToString()[..^Environment.NewLine.Length];
	}

	public void Dispose()
	{
		Console.SetOut(_originalOutput);
		_output.Dispose();
	}
}
