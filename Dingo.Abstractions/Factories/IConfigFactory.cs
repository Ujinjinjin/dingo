using Dingo.Abstractions.Config;

namespace Dingo.Abstractions.Factories
{
	public interface IConfigFactory
	{
		IGlobalConfig LoadGlobalConfig();
		IProjectConfig LoadProjectConfig();
	}
}
