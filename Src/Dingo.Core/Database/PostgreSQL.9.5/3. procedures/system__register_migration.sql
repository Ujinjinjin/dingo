create or replace function system__register_migration(
	p_migration_path text,
	p_migration_hash text,
	p_date_updated timestamp
)
returns void as
$$
begin
	insert into dingo_migration (
		migration_path,
		migration_hash,
		date_updated
	) values (
		p_migration_path,
		p_migration_hash,
		p_date_updated
	);
end;
$$
language plpgsql;
