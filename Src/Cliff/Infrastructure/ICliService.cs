using System;
using System.Threading.Tasks;

namespace Cliff.Infrastructure
{
	public interface ICliService
	{
		IServiceProvider ServiceProvider { get; }

		Task ExecuteAsync(string[] args);
	}
}