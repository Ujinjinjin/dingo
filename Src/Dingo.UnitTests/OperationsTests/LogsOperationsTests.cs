using AutoFixture;
using Dingo.Core.Helpers;
using Dingo.Core.Operations;
using Moq;
using System.IO;
using Xunit;

namespace Dingo.UnitTests.OperationsTests
{
	public class LogsOperationsTests : UnitTestsBase
	{
		[Fact]
		public void LogsOperationsTests__PruneLogsAsync__WhenLogsPathGiven_ThenAllFilesWithinPathDeleted()
		{
			// Arrange
			var logsDirectory = $"{Directory.GetCurrentDirectory()}/logs";
			
			var pathHelper = new Mock<IPathHelper>();
			pathHelper
				.Setup(x => x.GetLogsDirectory())
				.Returns(logsDirectory);
			
			var fixture = CreateFixture(pathHelper);

			var logsOperations = fixture.Create<LogsOperations>();

			Directory.CreateDirectory($"{logsDirectory}/1");
			Directory.CreateDirectory($"{logsDirectory}/2");
			File.Create($"{logsDirectory}/1/1.log").Close();
			File.Create($"{logsDirectory}/2/2.log").Close();

			// Act
			logsOperations.PruneLogsAsync().Wait();

			// Assert
			var di = new DirectoryInfo(logsDirectory);

			var filesCount = 0;
			foreach (var _ in di.EnumerateFiles())
			{
				filesCount++;
			}

			var dirsCount = 0;
			foreach (var _ in di.EnumerateDirectories())
			{
				dirsCount++;
			}
			
			Assert.Equal(0, filesCount);
			Assert.Equal(0, dirsCount);
		}
		
	}
}
