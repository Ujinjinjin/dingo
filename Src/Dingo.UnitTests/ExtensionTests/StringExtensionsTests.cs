using AutoFixture;
using Dingo.Core.Extensions;
using Xunit;

namespace Dingo.UnitTests.ExtensionTests
{
	public class StringExtensionsTests : UnitTestsBase
	{
		[Fact]
		public void StringExtensionsTests__ReplaceBackslashesWithSlashes__WhenStringWithBackSlashesGiven_ThenStringWithoutBackslashesReturned()
		{
			// Arrange
			var initialString = "\\\\/\\";

			// Act
			var result = initialString.ReplaceBackslashesWithSlashes();

			// Assert
			Assert.Equal("////", result);
		}

		[Fact]
		public void StringExtensionsTests__NotContains__WhenTwoStringsCompared_ThenInverseContainsResultReturned()
		{
			// Arrange
			var fixture = CreateFixture();
			var initialString = fixture.Create<string>();
			var substring = fixture.Create<string>();

			// Act
			var result = initialString.NotContains(substring);
			var expectedResult = !initialString.Contains(substring);

			// Assert
			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void StringExtensionsTests__ToUnixEol__WhenStringContainingWindowsEolGiven_ThenEolReplacedWithUnix()
		{
			// Arrange
			var initialString = "\n\r\n\r";

			// Act
			var result = initialString.ToUnixEol();

			// Assert
			Assert.Equal("\n\n\r", result);
		}
	}
}
