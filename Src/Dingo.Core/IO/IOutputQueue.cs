namespace Dingo.Core.IO
{
	/// <summary> Output queue interface </summary>
	public interface IOutputQueue
	{
		/// <summary> Put string in output queue </summary>
		/// <param name="outputValue">Item to queue</param>
		/// <param name="outputPath">Path where output will be written</param>
		void EnqueueOutput(string outputValue, string outputPath);

		/// <summary> Gets a value that indicates whether the IOutputQueue is empty </summary>
		bool IsEmpty { get; }
	}
}
