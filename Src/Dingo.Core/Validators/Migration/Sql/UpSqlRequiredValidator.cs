using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Validators.Primitive;

namespace Dingo.Core.Validators.Migration.Sql;

internal sealed class UpSqlRequiredValidator : ISqlCommandValidator
{
	private readonly StringRequiredValidator _stringRequiredValidator;

	public UpSqlRequiredValidator(StringRequiredValidator stringRequiredValidator)
	{
		_stringRequiredValidator = stringRequiredValidator.Required(nameof(stringRequiredValidator));
	}

	public bool Validate(MigrationCommand entity)
	{
		return _stringRequiredValidator.Validate(entity.Up);
	}
}
