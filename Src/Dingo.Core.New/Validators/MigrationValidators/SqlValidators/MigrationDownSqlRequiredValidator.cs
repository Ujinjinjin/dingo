using Dingo.Core.Models;
using Dingo.Core.Validators.Primitive;

namespace Dingo.Core.Validators.MigrationValidators.SqlValidators;

internal sealed class MigrationDownSqlRequiredValidator : IValidatorGroupMember<Migration, MigrationValidator>
{
	private readonly StringRequiredValidator _stringRequiredValidator;

	public MigrationDownSqlRequiredValidator(StringRequiredValidator stringRequiredValidator)
	{
		_stringRequiredValidator = stringRequiredValidator ?? throw new ArgumentNullException(nameof(stringRequiredValidator));
	}

	public bool Validate(Migration entity)
	{
		return _stringRequiredValidator.Validate(entity.Command.Down);
	}
}
