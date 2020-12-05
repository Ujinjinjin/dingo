using System.Threading.Tasks;

namespace Dingo.Core.Operations
{
	/// <summary> Logs operations </summary>
	public interface ILogsOperations
	{
		/// <summary> Prune log files </summary>
		Task PruneLogsAsync();
	}
}
