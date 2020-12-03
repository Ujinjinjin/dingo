namespace Dingo.Core.IO
{
	/// <summary> Output queue interface </summary>
	internal interface IOutputQueue
	{
		/// <summary> Put string in output queue </summary>
		/// <param name="outputValue">Item to queue</param>
		/// <param name="outputPath">Path where output will be written</param>
		void EnqueueOutput(string outputValue, string outputPath);
	}
}
