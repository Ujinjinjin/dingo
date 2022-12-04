using Dingo.Core.New.Validators.Primitive;

namespace Dingo.Core.New.Validators.MigrationValidator;

internal class MigrationUpSqlRequiredValidator : IValidator<Migration>
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
