using Dingo.Core.Migrations;
using Dingo.Core.Models;

namespace Dingo.Core.Validators.MigrationValidators;

internal sealed class MigrationValidator : ValidatorGroupHead<Migration, MigrationValidator>, IMigrationValidator
{
	public MigrationValidator(
		IEnumerable<IValidatorGroupMember<Migration, MigrationValidator>> groupMembers
	) : base(
		groupMembers
	)
	{
	}
}
