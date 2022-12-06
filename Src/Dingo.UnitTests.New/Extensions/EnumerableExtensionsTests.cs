using Dingo.Core.Extensions;

namespace Dingo.UnitTests.Extensions;

public class EnumerableExtensionsTests : UnitTestBase
{
	[Fact]
	public void EnumerableExtensionsTests_GetItem__WhenInBoundIndexGiven_ThenCorrespondingItemReturned()
	{
		// arrange
		var collection = Fixture.Create<IList<int>>();

		// act & assert
		for (var i = 0; i < collection.Count; i++)
		{
			var item = collection.GetItem(i);
			item.Should().Be(collection[i]);
		}
	}

	[Fact]
	public void EnumerableExtensionsTests_GetItem__WhenInBoundFromEndIndexGiven_ThenCorrespondingItemReturned()
	{
		// arrange
		var collection = Fixture.Create<IList<int>>();

		// act & assert
		for (var i = 1; i <= collection.Count; i++)
		{
			var item = collection.GetItem(^i);
			item.Should().Be(collection[^i]);
		}
	}

	[Theory]
	[InlineData(int.MaxValue, 0)]
	[InlineData(0, int.MaxValue)]
	public void EnumerableExtensionsTests_Sequence__WhenOutOfRangeIndexGiven_ThenExceptionThrown(int start, int end)
	{
		// arrange
		var collection = Fixture.Create<IList<int>>();

		// act
		var action = () => collection.Sequence(start..end);

		// assert
		action.Should().Throw<IndexOutOfRangeException>();
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Forward_N_to_M()
	{
		// arrange
		var range = 2..5;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 2, 3, 4 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Forward_N_to_rM()
	{
		// arrange
		var range = 2..^2;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 2, 3, 4, 5, 6, 7 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Forward_rN_to_M()
	{
		// arrange
		var range = ^5..8;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 5, 6, 7 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Forward_rN_to_rM()
	{
		// arrange
		var range = ^5..^2;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 5, 6, 7 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Forward_N_to_END()
	{
		// arrange
		var range = 5..;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 5, 6, 7, 8, 9 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Forward_rN_to_END()
	{
		// arrange
		var range = ^4..;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 6, 7, 8, 9 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Forward_START_to_N()
	{
		// arrange
		var range = ..4;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 0, 1, 2, 3 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Forward_START_to_END()
	{
		// arrange
		var range = ..;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Reverse_M_to_N()
	{
		// arrange
		var range = 5..2;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 4, 3, 2 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Reverse_rM_to_N()
	{
		// arrange
		var range = ^2..2;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 7, 6, 5, 4, 3, 2 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Reverse_M_to_rN()
	{
		// arrange
		var range = 8..^5;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 7, 6, 5 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void EnumerableExtensionsTests_Sequence__Reverse_rM_to_rN()
	{
		// arrange
		var range = ^2..^5;
		var collection = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		var expected = new[] { 7, 6, 5 };

		// act
		var sequence = collection.Sequence(range);

		// assert
		sequence.Should().BeEquivalentTo(expected);
	}
}
