using Dingo.Core.IO;

namespace Dingo.Core.Factories
{
	/// <inheritdoc />
	internal class OutputQueueFactory : IOutputQueueFactory
	{
		/// <inheritdoc />
		public IOutputQueue CreateFileOutputQueue() => new FileOutputQueue();
		
		/// <inheritdoc />
		public IOutputQueue CreateConsoleOutputQueue() => new ConsoleOutputQueue();
	}
}
