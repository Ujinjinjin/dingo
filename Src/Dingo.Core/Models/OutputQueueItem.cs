namespace Dingo.Core.Models
{
	/// <summary> Output queue item </summary>
	internal struct OutputQueueItem
	{
		/// <summary> Path where output will be written </summary>
		public string OutputPath { get; set; }

		/// <summary> Value of an output </summary>
		public string OutputValue { get; set; }
	}
}
