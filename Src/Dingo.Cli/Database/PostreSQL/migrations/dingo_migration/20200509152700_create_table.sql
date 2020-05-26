create table dingo_migration(
	dingo_migration_id integer,
	migration_path text,
	migration_hash char(256),
	date_updated date,
	primary key (dingo_migration_id)
);