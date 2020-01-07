using Dingo.Abstractions;
using Dingo.Core.Attributes;
using Dingo.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Reflection;

namespace Dingo.Core.Extensions
{
	public static class CliServiceExtensions
	{
		public static ICliService MapControllers(this ICliService service, RootCommand root)
		{
			var controllers = service.ServiceProvider
				.GetServices<IController>()
				.ToArray();

			foreach (var controller in controllers)
			{
				var controllerType = controller.GetType();
				
				var command = MapController(controllerType);

				foreach (var method in controllerType.GetMethods())
				{
					var subCommand = MapMethod(method);
					if (subCommand == null)
					{
						continue;
					}
					command.AddCommand(subCommand);
				}
				
				root.AddCommand(command);
			}
			
			return service;
		}

		private static Command MapController(MemberInfo controller)
		{
			var attributes = controller
				.GetCustomAttributes()
				.ToArray();

			var subCommandAttribute = attributes
				.OfType<SubCommandAttribute>()
				.SingleOrDefault();

			if (subCommandAttribute == null)
			{
				throw new SubCommandNotSpecifiedException();
			}

			var optionAttributes = attributes
				.OfType<OptionAttribute>()
				.ToArray();
				
			var command = new Command(subCommandAttribute.Name, subCommandAttribute.Description);

			foreach (var optionAttribute in optionAttributes)
			{
				command.AddOption(new Option(optionAttribute.Aliases, optionAttribute.Description));
			}

			return command;
		}

		private static Command MapMethod(MethodInfo method)
		{
			var attributes = method
				.GetCustomAttributes()
				.ToArray();
			
			var subCommandAttribute = attributes
				.OfType<SubCommandAttribute>()
				.SingleOrDefault();
			
			if (subCommandAttribute == null)
			{
				return null;
			}
			
			var optionAttributes = attributes
				.OfType<OptionAttribute>()
				.ToArray();
				
			var command = new Command(subCommandAttribute.Name, subCommandAttribute.Description);
			
			foreach (var optionAttribute in optionAttributes)
			{
				command.AddOption(new Option(optionAttribute.Aliases, optionAttribute.Description)
				{
					Argument = new Argument { ArgumentType = optionAttribute.Type }
				});
			}

			command.Handler = CommandHandler.Create(method);
			
			return command;
		}
	}
}
