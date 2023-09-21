-- Drop function
select dingo.drop_routine('_get_migrations_status', 'dingo');

-- Create function
create function dingo._get_migrations_status(
	pti_migration_info_input dingo.t_migration_info_input[]
)
returns table (
	"hash" varchar(256),
	hash_matches bool
) as
$$
begin
	----------------------------------------------------------------
	drop table if exists tt_input;
	----------------------------------------------------------------
	create temp table tt_input as
	----------------------------------------------------------------
	select
		cast(t1_input.migration_path as text) as migration_path,
		cast(t1_input.migration_hash as varchar(256)) as migration_hash
	from unnest(pti_migration_info_input) as t1_input;
	----------------------------------------------------------------
	return query select
		outer_table.migration_hash as "hash",
		outer_table.hash_matches
	from tt_input as inner_table
	cross join lateral (
		select
			tt_input.migration_hash,
			tt_input.migration_hash = dingo.migration.migration_hash as hash_matches
		from tt_input
		left outer join dingo.migration
			on tt_input.migration_path = dingo.migration.migration_path
		where 1 = 1
			and tt_input.migration_path = inner_table.migration_path
		order by dingo.migration.date_updated desc
		limit 1
	) as outer_table
	;
	----------------------------------------------------------------
end;
$$
language plpgsql;
