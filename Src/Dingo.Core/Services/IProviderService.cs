using System.Threading.Tasks;

namespace Dingo.Core.Services;

/// <summary> Operations with database provider </summary>
public interface IProviderService
{
	/// <summary> Let user choose database provider from supported list and save his choice to config file </summary>
	/// <param name="configPath">Custom path to configuration file</param>
	Task ChooseDatabaseProviderAsync(string configPath = null);
		
	/// <summary> Display list of supported database providers </summary>
	Task ListSupportedDatabaseProvidersAsync();
		
	/// <summary> Validate database provider specified in active configuration file </summary>
	/// <param name="configPath">Custom path to configuration file</param>
	Task ValidateDatabaseProviderAsync(string configPath = null);
}