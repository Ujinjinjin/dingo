using AutoFixture;
using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Xunit;

namespace Dingo.UnitTests.ExtensionTests
{
	public class MessageTypeExtensionsTests : UnitTestsBase
	{
		[Theory]
		[InlineData(MessageType.Error)]
		[InlineData(MessageType.Warning)]
		public void MessageTypeExtensionsTests__AddPrefixToMessage__WhenWarningOrErrorIsGiven_ThenMessagePrefixAdded(MessageType messageType)
		{
			// Arrange
			var fixture = CreateFixture();
			var message = fixture.Create<string>();
			var expectedString = $"{messageType}! {message}";

			// Act
			var result = messageType.AddPrefixToMessage(message);

			// Assert
			Assert.Equal(expectedString, result);
		}

		[Theory]
		[InlineData(MessageType.Unknown)]
		[InlineData(MessageType.Info)]
		public void MessageTypeExtensionsTests__AddPrefixToMessage__WhenInfoOrUnknownIsGiven_ThenMessagePrefixNotAdded(MessageType messageType)
		{
			// Arrange
			var fixture = CreateFixture();
			var message = fixture.Create<string>();

			// Act
			var result = messageType.AddPrefixToMessage(message);

			// Assert
			Assert.Equal(message, result);
		}

		[Theory]
		[InlineData(MessageType.Unknown, TextStyle.Plain)]
		[InlineData(MessageType.Info, TextStyle.Info)]
		[InlineData(MessageType.Warning, TextStyle.Warning)]
		[InlineData(MessageType.Error, TextStyle.Error)]
		public void MessageTypeExtensionsTests__ToTextStyle(MessageType messageType, TextStyle textStyle)
		{
			// Arrange
			// var fixture = CreateFixture();

			// Act
			var result = messageType.ToTextStyle();

			// Assert
			Assert.Equal(textStyle, result);
		}
	}
}
