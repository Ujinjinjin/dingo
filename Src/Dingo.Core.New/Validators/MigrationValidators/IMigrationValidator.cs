using Dingo.Core.Migrations;

namespace Dingo.Core.Validators.MigrationValidators;

internal interface IMigrationValidator : IValidatorGroupHead<Migration, IMigrationValidator>
{
}
