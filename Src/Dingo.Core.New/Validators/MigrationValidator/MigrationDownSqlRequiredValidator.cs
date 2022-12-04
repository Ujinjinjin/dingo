using Dingo.Core.New.Validators.Primitive;

namespace Dingo.Core.New.Validators.MigrationValidator;

internal class MigrationDownSqlRequiredValidator : IValidator<Migration>
{
	private readonly StringRequiredValidator _stringRequiredValidator;

	public MigrationDownSqlRequiredValidator(StringRequiredValidator stringRequiredValidator)
	{
		_stringRequiredValidator = stringRequiredValidator ?? throw new ArgumentNullException(nameof(stringRequiredValidator));
	}

	public bool Validate(Migration entity)
	{
		return _stringRequiredValidator.Validate(entity.DownSql);
	}
}
