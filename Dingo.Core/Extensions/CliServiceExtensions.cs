using Dingo.Abstractions;
using Dingo.Core.Attributes;
using Dingo.Core.Exceptions;
using Dingo.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
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
					if (subCommand.Command == null)
					{
						continue;
					}

					switch (subCommand.StackType)
					{
						case StackType.Nested:
							command.AddCommand(subCommand.Command);
							break;
						case StackType.Embedded:
							root.AddCommand(subCommand.Command);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
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

		private static SubCommand MapMethod(MethodInfo method)
		{
			var attributes = method
				.GetCustomAttributes()
				.ToArray();
			
			var subCommandAttribute = attributes
				.OfType<SubCommandAttribute>()
				.SingleOrDefault();
			
			if (subCommandAttribute == null)
			{
				return new SubCommand();
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
			
			return new SubCommand
			{
				Command = command,
				StackType = subCommandAttribute.StackType
			};
		}
	}
}
