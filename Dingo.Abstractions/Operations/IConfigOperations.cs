using System.Threading.Tasks;

namespace Dingo.Abstractions.Operations
{
	public interface IConfigOperations
	{
		Task ConfigDatabaseAsync(bool global, string connectionString, DatabaseEngine? dbEngine);
		void ShowConfigs(bool global);
	}
}
