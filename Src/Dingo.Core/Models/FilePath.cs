namespace Dingo.Core.Models
{
	/// <summary> Path to the file </summary>
	public struct FilePath
	{
		/// <summary> Absolute path to the file </summary>
		public string Absolute { get; set; }
		
		/// <summary> Relative path to the file </summary>
		public string Relative { get; set; }
		
		/// <summary> Name of database module </summary>
		public string Module { get; set; }
		
		/// <summary> Migration file name </summary>
		public string Filename { get; set; }
	}
}