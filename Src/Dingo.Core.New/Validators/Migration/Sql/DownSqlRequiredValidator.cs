using Dingo.Core.Extensions;
using Dingo.Core.Models;
using Dingo.Core.Validators.Primitive;
using Trico.Configuration;

namespace Dingo.Core.Validators.Migration.Sql;

internal sealed class DownSqlRequiredValidator : ISqlCommandValidator
{
	private readonly StringRequiredValidator _stringRequiredValidator;
	private readonly IConfiguration _configuration;

	public DownSqlRequiredValidator(
		StringRequiredValidator stringRequiredValidator,
		IConfiguration configuration
	)
	{
		_stringRequiredValidator =
			stringRequiredValidator ?? throw new ArgumentNullException(nameof(stringRequiredValidator));
		_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
	}

	public bool Validate(MigrationCommand entity)
	{
		return _configuration.IsFalse(Configuration.Key.MigrationDownRequired) ||
			_stringRequiredValidator.Validate(entity.Down);
	}
}
