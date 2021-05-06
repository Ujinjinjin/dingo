using Dingo.Core.Extensions;
using Dingo.Core.Models;
using System.Drawing;
using Xunit;

namespace Dingo.UnitTests.ExtensionTests
{
	public class TextStyleExtensionsTests : UnitTestsBase
	{
		[Theory]
		[InlineData(TextStyle.Info, false)]
		[InlineData(TextStyle.Warning, false)]
		[InlineData(TextStyle.Error, false)]
		[InlineData(TextStyle.Success, false)]
		[InlineData(TextStyle.Unknown, true)]
		[InlineData(TextStyle.Plain, true)]
		public void MessageTypeExtensionsTests__AddPrefixToMessage__WhenWarningOrErrorIsGiven_ThenMessagePrefixAdded(TextStyle textStyle, bool isEmptyColor)
		{
			// Arrange
			// var fixture = CreateFixture();

			// Act
			var result = textStyle.ToColor();

			// Assert
			if (isEmptyColor)
			{
				Assert.Equal(Color.Empty, result);				
			}
			else
			{
				Assert.NotEqual(Color.Empty, result);
			}
		}
	}
}
