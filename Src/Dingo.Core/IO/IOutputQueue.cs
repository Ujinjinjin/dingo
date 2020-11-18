namespace Dingo.Core.IO
{
	/// <summary> Output queue interface </summary>
	internal interface IOutputQueue
	{
		/// <summary> Put string in output queue </summary>
		/// <param name="outputPath">Path where output will be written</param>
		/// <param name="outputValue">Item to queue</param>
		void EnqueueOutput(string outputPath, string outputValue);
	}
}
