using Dingo.Abstractions;
using Dingo.Abstractions.Infrastructure;
using Dingo.Core.Attributes;
using Dingo.Core.Config;
using Dingo.Core.Exceptions;
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
				
				var commandInfo = MapController(controllerType);

				foreach (var method in controllerType.GetMethods())
				{
					var subCommandInfo = MapMethod(method, controller);
					if (subCommandInfo.Command == null)
					{
						continue;
					}

					switch (subCommandInfo.StackType)
					{
						case StackType.Nested:
							if (commandInfo.StackType == StackType.Hidden)
							{
								throw new InvalidStackTypeException($"Hidden controllers can't have any nested commands. Controller: {controllerType.FullName}");
							}
							commandInfo.Command.AddCommand(subCommandInfo.Command);
							break;
						case StackType.Embedded:
							root.AddCommand(subCommandInfo.Command);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}

				if (commandInfo.StackType == StackType.Hidden)
				{
					return service;
				}
				root.AddCommand(commandInfo.Command);
			}
			
			return service;
		}

		private static CommandInfo MapController(MemberInfo controller)
		{
			try
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

				return new CommandInfo
				{
					Command = command,
					StackType = subCommandAttribute.StackType
				};
			}
			catch (Exception exception)
			{
				throw new FaultException($"Error occured while mapping controller {controller.Name}", exception);
			}
		}

		private static CommandInfo MapMethod<T>(MethodInfo method, T controller)
		{
			try
			{
				var attributes = method
					.GetCustomAttributes()
					.ToArray();
			
				var subCommandAttribute = attributes
					.OfType<SubCommandAttribute>()
					.SingleOrDefault();
			
				if (subCommandAttribute == null)
				{
					return new CommandInfo();
				}
			
				var optionAttributes = attributes
					.OfType<OptionAttribute>()
					.ToArray();
				
				var command = new Command(subCommandAttribute.Name, subCommandAttribute.Description);
			
				foreach (var optionAttribute in optionAttributes)
				{
					command.AddOption(new Option(optionAttribute.Aliases, optionAttribute.Description)
					{
						Argument = new Argument { ArgumentType = optionAttribute.Type },
						Required = optionAttribute.Required
					});
				}

				command.Handler = CommandHandler.Create(method, controller);
			
				return new CommandInfo
				{
					Command = command,
					StackType = subCommandAttribute.StackType
				};
			}
			catch (Exception exception)
			{
				throw new FaultException($"Error occured while mapping method {method.Name} in module {method.Module}", exception);
			}
		}
	}
}
