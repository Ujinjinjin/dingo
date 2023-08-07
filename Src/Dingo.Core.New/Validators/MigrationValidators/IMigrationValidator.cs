using Dingo.Core.Migrations;
using Dingo.Core.Models;

namespace Dingo.Core.Validators.MigrationValidators;

internal interface IMigrationValidator : IValidatorGroupHead<Migration, IMigrationValidator>
{
}
