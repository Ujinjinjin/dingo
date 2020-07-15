using AutoFixture;
using Dingo.Core.Extensions;
using Dingo.UnitTests.Base;
using System;
using System.Linq;
using Xunit;

namespace Dingo.UnitTests.ExtensionTests
{
	public class ListExtensionsTests : UnitTestsBase
	{
		[Fact]
		public void ListExtensionsTests__GetItem__WhenNegativeIndexGiven_ThenIndexCountGoesBackwards()
		{
			// Arrange
			var fixture = new Fixture();
			var array = CreateIntArray(fixture.Create<int>());
			var index = new Random().Next(0, array.Count);
			
			// Act
			var result = array.GetItem(index);

			// Assert
			Assert.Equal(array[index], result);
		}

		[Fact]
		public void ListExtensionsTests__Sequence__WhenValidIndicesGiven_ThenReturnedSequence()
		{
			// Arrange
			var fixture = new Fixture();
			var array = CreateIntArray(fixture.Create<int>());
			var startIndex = new Random().Next(0, array.Count / 2);
			var endIndex = new Random().Next(array.Count / 2, array.Count);
			
			// Act
			var result = array.Sequence(startIndex, endIndex);
			var expectedValue = array
				.Skip(startIndex)
				.Take(endIndex - startIndex)
				.ToArray();

			// Assert
			Assert.Equal(expectedValue, result);
		}

		[Fact]
		public void ListExtensionsTests__Sequence__WhenEndIndexOutOfRangeGiven_ThenExceptionThrown()
		{
			// Arrange
			var fixture = new Fixture();
			var array = CreateIntArray(fixture.Create<int>());
			var startIndex = new Random().Next(0, array.Count / 2);
			var endIndex = array.Count * 2;

			// Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => array.Sequence(startIndex, endIndex));
		}

		[Fact]
		public void ListExtensionsTests__SequenceFrom__WhenValidIndicesGiven_ThenSequenceReturnedFromStartIndexToEnd()
		{
			// Arrange
			var fixture = new Fixture();
			var array = CreateIntArray(fixture.Create<int>());
			var startIndex = new Random().Next(0, array.Count / 2);

			// Act
			var result = array.SequenceFrom(startIndex);
			var expectedValue = array.Sequence(startIndex, ^1);
			
			// Assert
			Assert.Equal(expectedValue, result);
		}

		[Fact]
		public void ListExtensionsTests__SequenceTo__WhenValidIndicesGiven_ThenSequenceReturnedFromBeginningToEndIndex()
		{
			// Arrange
			var fixture = new Fixture();
			var array = CreateIntArray(fixture.Create<int>());
			var endIndex = new Random().Next(array.Count / 2, array.Count);

			// Act
			var result = array.SequenceTo(endIndex);
			var expectedValue = array.Sequence(0, endIndex);
			
			// Assert
			Assert.Equal(expectedValue, result);
		}
	}
}
