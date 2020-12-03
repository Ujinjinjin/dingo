using Dingo.Core.IO;

namespace Dingo.Core.Factories
{
	/// <summary> Output queue factory </summary>
	internal interface IOutputQueueFactory
	{
		/// <summary> Create queue with output to file </summary>
		/// <returns>Output queue</returns>
		IOutputQueue CreateFileOutputQueue();

		/// <summary> Create queue with output to console </summary>
		/// <returns>Output queue</returns>
		IOutputQueue CreateConsoleOutputQueue();
	}
}
