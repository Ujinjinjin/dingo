-- Drop function
select dingo.drop_routine('_get_migrations_status', 'dingo');

-- Create function
create function dingo._get_migrations_status(
	pti_migration_info_input dingo.t_migration_info_input[]
)
returns table (
	migration_hash varchar(256),
	hash_matches bool
) as
$$
begin
	----------------------------------------------------------------
	drop table if exists tt_input;
	----------------------------------------------------------------
	create temp table tt_input as
	select
		cast(t1_input.migration_path as text) as migration_path,
		cast(t1_input.migration_hash as varchar(256)) as migration_hash
	from unnest(pti_migration_info_input) as t1_input;
	----------------------------------------------------------------
	return query select
		t1_input.migration_hash,
		t1_input.migration_hash = registered_migration.migration_hash as hash_matches
	from tt_input as t1_input
	left outer join (
		select
			migration.migration_path,
			patch_migration.migration_hash
		from dingo.migration as migration
		inner join dingo.patch_migration as patch_migration
			on patch_migration.migration_id = migration.migration_id
		inner join dingo.patch as patch
			on patch.patch_number = patch_migration.patch_number
			and patch.reverted = false
	) as registered_migration on registered_migration.migration_path = t1_input.migration_path
	;
	----------------------------------------------------------------
end;
$$
language plpgsql;
