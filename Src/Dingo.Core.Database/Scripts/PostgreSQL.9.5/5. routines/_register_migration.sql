-- Drop function
select dingo.drop_routine('_register_migration', 'dingo');

-- Create function
create function dingo._register_migration(
	p_migration_path text,
	p_migration_hash varchar(256),
	p_date_updated timestamp
)
returns void as
$$
begin
	----------------------------------------------------------------
	insert into dingo.migration (
		migration_path,
		migration_hash,
		date_updated
	) values (
		p_migration_path,
		p_migration_hash,
		p_date_updated
	);
	----------------------------------------------------------------
end;
$$
language plpgsql;
