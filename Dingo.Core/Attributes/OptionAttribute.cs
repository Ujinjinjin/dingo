using System;

namespace Dingo.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class OptionAttribute : Attribute
	{
		public string[] Aliases { get; }
		public string Description { get; }
		public Type Type { get; }

		public OptionAttribute(string description, Type type, params string[] aliases)
		{
			Description = description;
			Type = type;
			Aliases = aliases;
		}
	}
}
