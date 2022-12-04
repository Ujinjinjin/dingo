using Dingo.Core.Validators.Primitive;

namespace Dingo.Core.Validators.MigrationValidators.SqlValidators;

internal sealed class MigrationUpSqlRequiredValidator : IValidatorGroupMember<Migration, MigrationValidator>
{
	private readonly StringRequiredValidator _stringRequiredValidator;

	public MigrationUpSqlRequiredValidator(StringRequiredValidator stringRequiredValidator)
	{
		_stringRequiredValidator = stringRequiredValidator ?? throw new ArgumentNullException(nameof(stringRequiredValidator));
	}

	public bool Validate(Migration entity)
	{
		return _stringRequiredValidator.Validate(entity.UpSql);
	}
}
