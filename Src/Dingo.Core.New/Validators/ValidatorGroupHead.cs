using Dingo.Core.Extensions;

namespace Dingo.Core.Validators;

internal abstract class ValidatorGroupHead<T, TGroup> : Validator<T>, IValidatorGroupHead<T, TGroup>
{
	private readonly IEnumerable<IValidatorGroupMember<T, TGroup>> _groupMembers;

	protected ValidatorGroupHead(IEnumerable<IValidatorGroupMember<T, TGroup>> groupMembers)
	{
		_groupMembers = groupMembers.Required(nameof(groupMembers));
	}

	protected override IReadOnlyList<Func<T, bool>> GetValidationRules()
	{
		var validationRules = new List<Func<T, bool>>();

		foreach (var member in _groupMembers)
		{
			if (member is IValidatorGroupHead<T, TGroup>)
			{
				continue;
			}

			validationRules.Add(m => member.Validate(m));
		}

		return validationRules;
	}
}
