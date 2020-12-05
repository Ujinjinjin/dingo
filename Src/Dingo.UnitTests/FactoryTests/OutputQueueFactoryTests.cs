using AutoFixture;
using Dingo.Core.Factories;
using Dingo.Core.IO;
using Xunit;

namespace Dingo.UnitTests.FactoryTests
{
	public class OutputQueueFactoryTests : UnitTestsBase
	{
		[Fact]
		public void OutputQueueFactoryTests__CreateFileOutputQueue__WhenFactoryMethodInvoked_ThenFileOutputQueueReturned()
		{
			// Arrange
			var fixture = CreateFixture();
			var outputQueueFactory = fixture.Create<OutputQueueFactory>();

			// Act
			var fileOutputQueue = outputQueueFactory.CreateFileOutputQueue();

			// Assert
			Assert.NotNull(fileOutputQueue);
			Assert.IsAssignableFrom<IOutputQueue>(fileOutputQueue);
			Assert.Equal(typeof(FileOutputQueue), fileOutputQueue.GetType());
		}
		
		[Fact]
		public void OutputQueueFactoryTests__CreateConsoleOutputQueue__WhenFactoryMethodInvoked_ThenConsoleOutputQueueReturned()
		{
			// Arrange
			var fixture = CreateFixture();
			var outputQueueFactory = fixture.Create<OutputQueueFactory>();

			// Act
			var consoleOutputQueue = outputQueueFactory.CreateConsoleOutputQueue();

			// Assert
			Assert.NotNull(consoleOutputQueue);
			Assert.IsAssignableFrom<IOutputQueue>(consoleOutputQueue);
			Assert.Equal(typeof(ConsoleOutputQueue), consoleOutputQueue.GetType());
		}
	}
}
