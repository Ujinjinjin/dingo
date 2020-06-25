using Dingo.Core.Config;

namespace Dingo.Core.Models
{
	public struct LoadConfigResult
	{
		public string ConfigPath { get; set; }
		public IConfiguration Configuration { get; set; }
	}
}
