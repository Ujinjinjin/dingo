create or replace function system__add_migration(
	p_migration_path text,
	p_migration_hash char(256),
	p_date_updated date
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
