using Dingo.Core.Extensions;
using Dingo.Core.Services.Adapters;
using Dingo.Core.Utils;
using Trico.Configuration;

namespace Dingo.Core.Services.Config;

internal sealed class ConfigProfileLoader : IConfigProfileLoader
{
	private readonly IConfiguration _configuration;
	private readonly IPath _path;

	public ConfigProfileLoader(IConfiguration configuration, IPath path)
	{
		_configuration = configuration.Required(nameof(configuration));
		_path = path.Required(nameof(path));
	}

	public async Task LoadAsync(string? profile, CancellationToken ct = default)
	{
		var configFilename = ConfigFilename.Build(profile);
		var configFilepath = _path.Join(Constants.CurrentDir, Constants.ConfigDir, configFilename);

		var options = new Dictionary<string, string>();
		foreach (var (key, value) in Configuration.Dict)
		{
			options.Add(key, value);
		}
		options.Add("config-filepath", configFilepath);
		options.Add("prefix", Constants.ConfigEnvPrefix);

		await _configuration.LoadAsync(options, ct);
	}
}
