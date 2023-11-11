using Dingo.Core.Models;
using Dingo.Core.Services.Migrations;

namespace Dingo.UnitTests.Services.Migrations;

public class MigrationStatusCalculatorTests : UnitTestBase
{
	[Fact]
	public void MigrationStatusCalculatorTests_NeedToApplyMigration__WhenMigrationIsUpToDate_ThenDoNotNeedToApply()
	{
		// arrange
		var calculator = new MigrationStatusCalculator();
		var migration = CreateMigration(MigrationStatus.UpToDate);

		// act
		var needToApply = calculator.NeedToApplyMigration(migration);

		// assert
		needToApply.Should().Be(false);
	}

	[Theory]
	[InlineData(MigrationStatus.New)]
	[InlineData(MigrationStatus.Outdated)]
	[InlineData(MigrationStatus.ForceOutdated)]
	public void MigrationStatusCalculatorTests_NeedToApplyMigration__WhenMigrationIsNotUpToDate_ThenNeedToApply(MigrationStatus status)
	{
		// arrange
		var calculator = new MigrationStatusCalculator();
		var migration = CreateMigration(status);

		// act
		var needToApply = calculator.NeedToApplyMigration(migration);

		// assert
		needToApply.Should().Be(true);
	}

	[Fact]
	public void MigrationStatusCalculatorTests_CalculatePatchMigrationStatus__WhenLocalMigrationNotExists_ThenWarningStatusReturned()
	{
		// arrange
		var patchMigration = CreatePatchMigration();
		var localMigrationMap = new Dictionary<string, Migration>();
		var calculator = new MigrationStatusCalculator();

		// act
		var status = calculator.CalculatePatchMigrationStatus(patchMigration, localMigrationMap);

		// assert
		status.Should().NotHaveFlag(PatchMigrationStatus.Ok);
		status.Should().HaveFlag(PatchMigrationStatus.Warning);
		status.Should().HaveFlag(PatchMigrationStatus.LocalMigrationNotFound);
	}

	[Fact]
	public void MigrationStatusCalculatorTests_CalculatePatchMigrationStatus__WhenLocalMigrationIsOutdated_ThenWarningStatusReturned()
	{
		// arrange
		var patchMigration = CreatePatchMigration();
		var localMigrationMap = new Dictionary<string, Migration>()
		{
			{ patchMigration.MigrationPath, CreateMigration(MigrationStatus.Outdated) },
		};
		var calculator = new MigrationStatusCalculator();

		// act
		var status = calculator.CalculatePatchMigrationStatus(patchMigration, localMigrationMap);

		// assert
		status.Should().NotHaveFlag(PatchMigrationStatus.Ok);
		status.Should().HaveFlag(PatchMigrationStatus.Warning);
		status.Should().HaveFlag(PatchMigrationStatus.LocalMigrationModified);
	}

	[Fact]
	public void MigrationStatusCalculatorTests_CalculatePatchMigrationStatus__WhenLocalMigrationHashDiffersFromPatch_ThenWarningStatusReturned()
	{
		// arrange
		var patchMigration = CreatePatchMigration();
		var localMigration = CreateMigration(MigrationStatus.UpToDate);
		var localMigrationMap = new Dictionary<string, Migration>()
		{
			{ patchMigration.MigrationPath, localMigration },
		};
		var calculator = new MigrationStatusCalculator();

		// act
		var status = calculator.CalculatePatchMigrationStatus(patchMigration, localMigrationMap);

		// assert
		patchMigration.MigrationHash.Should().NotBe(localMigration.Hash.Value);
		status.Should().NotHaveFlag(PatchMigrationStatus.Ok);
		status.Should().HaveFlag(PatchMigrationStatus.Warning);
		status.Should().HaveFlag(PatchMigrationStatus.LocalMigrationModified);
	}

	[Fact]
	public void MigrationStatusCalculatorTests_CalculatePatchMigrationStatus__WhenLocalMigrationIsUpToDate_ThenOkStatusReturned()
	{
		// arrange
		var patchMigration = CreatePatchMigration();
		var localMigration = CreateMigration(MigrationStatus.UpToDate, patchMigration.MigrationHash);
		var localMigrationMap = new Dictionary<string, Migration>()
		{
			{ patchMigration.MigrationPath, localMigration },
		};
		var calculator = new MigrationStatusCalculator();

		// act
		var status = calculator.CalculatePatchMigrationStatus(patchMigration, localMigrationMap);

		// assert
		status.Should().HaveFlag(PatchMigrationStatus.Ok);
		status.Should().NotHaveFlag(PatchMigrationStatus.Warning);
	}
}
