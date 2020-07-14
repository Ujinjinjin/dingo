using AutoFixture;
using Dingo.Core.Extensions;
using Xunit;

namespace Dingo.UnitTests.ExtensionTests
{
	public class IntegerExtensionsTests
	{
		[Fact]
		public void IntegerExtensionsTests__Negate__WhenNotZeroIntegerGiven_ThenIntegerNegated()
		{
			// Arrange
			var fixture = new Fixture();
			var initialNumber = fixture.Create<int>();
			
			// Act
			var result = initialNumber.Negate();

			// Assert
			Assert.Equal(-initialNumber, result);
		}
	}
}
