using Dingo.Cli.Infrastructure;
using Dingo.InfrastructureTests.Helpers;

namespace Dingo.InfrastructureTests;

public class SmokeTests
{
	[Fact]
	public async Task Smoke()
	{
		// arrange
		var output = Guid.NewGuid().ToString();

		using var consoleOutput = new ConsoleOutput();

		// act
		await Program.Main(new[] { "test", "subcommand", "-o", output });

		// assert
		Assert.Equal(output, consoleOutput.Get());
	}
}
