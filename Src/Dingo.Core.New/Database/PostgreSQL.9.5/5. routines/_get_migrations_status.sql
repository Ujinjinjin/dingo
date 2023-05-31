-- Drop function
select dingo.drop_routine('_get_migrations_status', 'dingo');

-- Create function
create function dingo._get_migrations_status(
	pti_migration_info_input t_migration_info_input[]
)
returns table (
	migration_path text,
	new_hash varchar(256),
	old_hash varchar(256),
	is_outdated bool,
	date_updated timestamp
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
		outer_table.migration_path,
		outer_table.new_hash,
		outer_table.old_hash,
		outer_table.is_outdated,
		outer_table.date_updated
	from tt_input as inner_table
	cross join lateral (
		select
			tt_input.migration_path,
			tt_input.migration_hash as new_hash,
			dingo.migration.migration_hash as old_hash,
			tt_input.migration_hash !~ dingo.migration.migration_hash as is_outdated,
			dingo.migration.date_updated
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
