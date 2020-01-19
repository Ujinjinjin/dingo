using System;
using System.Threading.Tasks;

namespace Dingo.Abstractions.Infrastructure
{
	public interface ICliService
	{
		IServiceProvider ServiceProvider { get; }

		Task ExecuteAsync(string[] args);
	}
}
