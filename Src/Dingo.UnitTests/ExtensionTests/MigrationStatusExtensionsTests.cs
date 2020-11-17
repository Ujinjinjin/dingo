using AutoFixture;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using System;
using Xunit;

namespace Dingo.UnitTests.ExtensionTests
{
	public class MigrationStatusExtensionsTests : UnitTestsBase
	{
		[Fact]
		public void MigrationStatusExtensionsTests__ToDisplayText__WhenValidEnumValueGiven_ThenDisplayTextReturned()
		{
			// Arrange
			var fixture = CreateFixture();
			var enumValue = fixture.Create<MigrationStatus>();
			
			// Act
			var result = enumValue.ToDisplayText();

			// Assert
			Assert.Equal(typeof(string), result.GetType());
		}
		
		[Fact]
		public void MigrationStatusExtensionsTests__ToDisplayText__WhenInvalidEnumValueGiven_ThenExceptionThrown()
		{
			// Arrange
			var enumValue = (MigrationStatus) int.MaxValue;
			
			// Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => enumValue.ToDisplayText());
		}
	}
}
