using AutoFixture;
using Dingo.Cli.Infrastructure;
using Dingo.InfrastructureTests.Helpers;
using FluentAssertions;

namespace Dingo.InfrastructureTests;

public class SmokeTests
{
	[Fact]
	public async Task Smoke()
	{
		// arrange
		var fixture = new Fixture();
		var output = fixture.Create<string>();

		using var consoleOutput = new ConsoleOutput();

		// act
		await Program.Main(new[] { "test", "subcommand", "-o", output });
		var result = consoleOutput.Get();

		// assert
		result.Should().Be(output);
	}
}
