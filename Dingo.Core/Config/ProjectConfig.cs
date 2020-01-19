using Dingo.Abstractions;
using Dingo.Abstractions.Config;
using Dingo.Core.Models;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Dingo.Core.Config
{
	public class ProjectConfig : BaseConfig, IProjectConfig
	{
		public ProjectConfig(
			IDeserializer deserializer, 
			ISerializer serializer
		) : base(deserializer, serializer)
		{
			DingoDirectory = GetConfigDirectory(Directory.GetCurrentDirectory());
			LoadAsync().Wait();
		}
		
		public async Task UpdateAsync()
		{
			await base.UpdateAsync(this, DingoDirectory);
		}

		public void Initialize()
		{
			base.Initialize(DingoDirectory);
		}

		public async Task LoadAsync()
		{
			await base.LoadAsync(this, DingoDirectory);
		}

		public string ConnectionString { get; set; }
		public DatabaseEngine? DatabaseEngine { get; set; }
		
		[YamlIgnore]
		public string DingoDirectory { get; }
	}
}
