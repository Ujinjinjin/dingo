-- Drop function
select dingo.drop_routine('_get_last_patch', 'dingo');

-- Create function
create function dingo._get_last_patch(
	p_patch_count integer = 1
)
returns table (
	migration_hash varchar(256),
	migration_path varchar(512),
	patch_number integer
) as
$$
declare
	user_patch integer := 2;
begin
	----------------------------------------------------------------
	drop table if exists tt_patches;
	----------------------------------------------------------------
	create temp table tt_patches as
	select patch.patch_number
	from dingo.patch as patch
	where 1 = 1
		and patch.reverted = false
		and patch."type" = user_patch
	order by patch.patch_number desc
	limit p_patch_count;
	----------------------------------------------------------------
	return query select
		patch_migration.migration_hash,
		migration.migration_path,
		patch_migration.patch_number
	from dingo.patch_migration as patch_migration
	inner join dingo.migration as migration
		on migration.migration_id = patch_migration.migration_id
	inner join tt_patches
		on tt_patches.patch_number = patch_migration.patch_number
	;
	----------------------------------------------------------------
end;
$$
language plpgsql;
