using Dingo.Abstractions;
using Dingo.Abstractions.Config;
using JetBrains.Annotations;

namespace Dingo.Core.Models
{
	[UsedImplicitly]
	internal class ConfigModel : IDingoConfig
	{
		public string ConnectionString { get; set; }
		public DatabaseEngine? DatabaseEngine { get; set; }
		public string DingoDirectory { get; }
	}
}
