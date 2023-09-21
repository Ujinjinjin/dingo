using Dingo.Core.Exceptions;
using Dingo.Core.Extensions;

namespace Dingo.UnitTests.Extensions;

public class ObjectExtensions : UnitTestBase
{
	[Fact]
	public void ObjectExtensions_Required__WhenObjectIsNull_ThenExceptionThrown()
	{
		// arrange
		object? obj = null;

		// act
		var func = () => obj.Required(nameof(obj));

		// assert
		func.Should().Throw<ValueRequiredException>();
	}

	[Fact]
	public void ObjectExtensions_Required__WhenObjectIsNotNull_ThenTheSameObjectReturned()
	{
		// arrange
		object obj = Fixture.Create<object>();

		// act
		var res = obj.Required(nameof(obj));

		// assert
		res.Should().Be(obj);
	}
}
