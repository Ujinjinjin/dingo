using Dingo.Abstractions;
using Dingo.Core.Attributes;

namespace Dingo.Cli.Controllers
{
	[SubCommand(StackType = StackType.Hidden)]
	internal class ConfigController : IController
	{
		[SubCommand("config", "Configure dingo", StackType.Embedded)]
		public void Config()
		{
			
		}
	}
}
