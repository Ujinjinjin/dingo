using System;
using System.Threading.Tasks;

namespace Dingo.Abstractions
{
	public interface ICliService
	{
		IServiceProvider ServiceProvider { get; }

		Task ExecuteAsync(string[] args);
	}
}
