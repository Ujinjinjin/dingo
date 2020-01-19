using Dingo.Abstractions;
using Dingo.Abstractions.Config;
using System;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Dingo.Core.Config
{
	public class GlobalConfig : BaseConfig, IGlobalConfig
	{
		public GlobalConfig(
			IDeserializer deserializer, 
			ISerializer serializer
		) : base(deserializer, serializer)
		{
			DingoDirectory = GetConfigDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
			LoadAsync().Wait();
		}

		public async Task UpdateAsync()
		{
			await UpdateAsync(this, DingoDirectory);
		}

		public void Initialize()
		{
			Initialize(DingoDirectory);
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
