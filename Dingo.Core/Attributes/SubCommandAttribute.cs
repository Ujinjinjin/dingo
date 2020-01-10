using Dingo.Abstractions;
using System;

namespace Dingo.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class SubCommandAttribute : Attribute
	{
		private const string DefaultName = "default";
		
		private readonly string _name;
		private string _description;
		private StackType _stackType;

		public SubCommandAttribute()
		{
			_name = DefaultName;
			_stackType = StackType.Nested;
		}
		
		public SubCommandAttribute(string name, string description = null, StackType stackType = StackType.Nested)
		{
			_name = name ?? throw new ArgumentNullException(nameof(name));
			_description = description;
			_stackType = stackType;
		}

		public string Name => _name;

		public string Description
		{
			get => _description;
			set => _description = value;
		}

		public StackType StackType
		{
			get => _stackType;
			set => _stackType = value;
		}
	}
}
