create or alter procedure dingo._get_last_patch(
	@p_patch_count integer = 1
)
as
begin
	----------------------------------------------------------------
	drop table if exists #tt_patches;
	----------------------------------------------------------------
	declare @user_patch int = 2;
	----------------------------------------------------------------
	select top (@p_patch_count)
		patch.patch_number
	into #tt_patches
	from dingo.patch as patch
	where 1 = 1
		and patch.reverted = 0
		and patch.[type] = @user_patch
	order by patch.patch_number desc;
	----------------------------------------------------------------
	select
		patch_migration.migration_hash,
		migration.migration_path,
		patch_migration.patch_number
	from dingo.patch_migration as patch_migration
	inner join dingo.migration as migration
		on migration.migration_id = patch_migration.migration_id
	inner join #tt_patches as tt_patches
		on tt_patches.patch_number = patch_migration.patch_number
	;
	----------------------------------------------------------------
end;
