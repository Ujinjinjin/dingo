using Dingo.Core.Validators.Migration.Sql;

namespace Dingo.Core.Validators.Migration;

internal sealed class MigrationValidator : IMigrationValidator
{
	private readonly IEnumerable<ISqlCommandValidator> _sqlCommandValidators;

	public MigrationValidator(
		IEnumerable<ISqlCommandValidator> sqlCommandSqlCommandValidators
	)
	{
		_sqlCommandValidators = sqlCommandSqlCommandValidators;
	}

	public bool Validate(Models.Migration entity)
	{
		return _sqlCommandValidators.All(x => x.Validate(entity.Command));
	}
}
