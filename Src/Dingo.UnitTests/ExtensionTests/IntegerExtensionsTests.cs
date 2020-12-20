using AutoFixture;
using Dingo.Core.Extensions;
using Xunit;

namespace Dingo.UnitTests.ExtensionTests
{
	public class IntegerExtensionsTests : UnitTestsBase
	{
		[Fact]
		public void IntegerExtensionsTests__Negate__WhenNotZeroIntegerGiven_ThenIntegerNegated()
		{
			// Arrange
			var fixture = CreateFixture();
			var initialNumber = fixture.Create<int>();

			// Act
			var result = initialNumber.Negate();

			// Assert
			Assert.Equal(-initialNumber, result);
		}
	}
}
