using System;

namespace Dingo.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class OptionAttribute : Attribute
	{
		private string _description;
		private readonly Type _type;
		private readonly string[] _aliases;

		public OptionAttribute(Type type, params string[] aliases)
		{
			_description = null;
			_type = type ?? throw new ArgumentNullException(nameof(type));
			_aliases = aliases;
		}

		public OptionAttribute(string description, Type type, params string[] aliases)
		{
			_description = description;
			_type = type ?? throw new ArgumentNullException(nameof(type));
			_aliases = aliases;
		}

		public string Description
		{
			get => _description;
			set => _description = value;
		}

		public Type Type => _type;

		public string[] Aliases => _aliases;
	}
}
