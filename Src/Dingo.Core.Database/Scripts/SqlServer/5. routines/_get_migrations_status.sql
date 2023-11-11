create or alter procedure dingo._get_migrations_status(
	@pti_migration_info_input dingo.t_migration_info_input readonly
)
as
begin
	----------------------------------------------------------------
	declare @c_unreal_char char = -1;
	----------------------------------------------------------------
	select
		inner_table.migration_hash,
		case
			when outer_table.migration_hash is null then null
			else iif(isnull(inner_table.migration_hash, @c_unreal_char) = isnull(outer_table.migration_hash, @c_unreal_char), 1, 0)
		end as hash_matches
	from @pti_migration_info_input as inner_table
	outer apply (
		select top(1)
			migration.migration_path,
			patch_migration.migration_hash
		from dingo.migration as migration
		inner join dingo.patch_migration as patch_migration
			on patch_migration.migration_id = migration.migration_id
		inner join dingo.patch as patch
			on patch.patch_number = patch_migration.patch_number
			and patch.reverted = 0
		where migration.migration_path = inner_table.migration_path
		order by patch.patch_number desc
	) as outer_table
	;
	----------------------------------------------------------------
end;
