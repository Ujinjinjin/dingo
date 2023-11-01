using Dingo.Core.Extensions;

namespace Dingo.UnitTests.Extensions;

public class StringExtensionsTests : UnitTestBase
{
	[Fact]
	public void StringExtensionsTests__NotContains__WhenTwoStringsCompared_ThenInverseContainsResultReturned()
	{
		// Arrange
		var initialString = Fixture.Create<string>();
		var substring = Fixture.Create<string>();

		// Act
		var result = initialString.NotContains(substring);
		var expectedResult = !initialString.Contains(substring);

		// Assert
		result.Should().Be(expectedResult);
	}

	[Fact]
	public void StringExtensionsTests__ToUnixEol__WhenStringContainingWindowsEolGiven_ThenEolReplacedWithUnix()
	{
		// Arrange
		var initialString = "\n\r\n\r";
		var expected = "\n\n\r";

		// Act
		var result = initialString.ToUnixEol();

		// Assert
		result.Should().Be(expected);
	}
}
