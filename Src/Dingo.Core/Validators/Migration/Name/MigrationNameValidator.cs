using System.Text.RegularExpressions;
using Dingo.Core.Extensions;
using Dingo.Core.Validators.Primitive;

namespace Dingo.Core.Validators.Migration.Name;

internal class MigrationNameValidator : IMigrationNameValidator
{
	private readonly StringRequiredValidator _stringRequiredValidator;
	private readonly Regex _regex = new(@"^[\w\d_]+$");

	public MigrationNameValidator(StringRequiredValidator stringRequiredValidator)
	{
		_stringRequiredValidator = stringRequiredValidator.Required(nameof(stringRequiredValidator));
	}

	public bool Validate(string? entity)
	{
		return _stringRequiredValidator.Validate(entity) && _regex.IsMatch(entity!);
	}
}
