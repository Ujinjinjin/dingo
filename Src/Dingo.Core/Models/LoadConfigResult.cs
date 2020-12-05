using Dingo.Core.Config;

namespace Dingo.Core.Models
{
	/// <summary> Result of loading config file </summary>
	public struct LoadConfigResult
	{
		/// <summary> Path from which configs were loaded </summary>
		public string ConfigPath { get; set; }

		/// <summary> Project config </summary>
		public IConfiguration Configuration { get; set; }
	}
}
