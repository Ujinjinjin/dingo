create table dingo_migration(
	dingo_migration_id serial,
	migration_path text,
	migration_hash text,
	date_updated timestamp,
	primary key (dingo_migration_id)
);