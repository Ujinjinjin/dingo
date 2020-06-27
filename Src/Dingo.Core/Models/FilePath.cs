namespace Dingo.Core.Models
{
	/// <summary> Path to the file </summary>
	internal struct FilePath
	{
		/// <summary> Absolute path to the file </summary>
		public string Absolute { get; set; }
		
		/// <summary> Relative path to the file </summary>
		public string Relative { get; set; }
	}
}