if dingo.exists_table('migration', 'dingo') = 0
begin
	create table dingo.migration(
		migration_id int not null identity(1, 1),
		migration_path nvarchar(max) not null,
		migration_hash varchar(256) not null,
		date_updated datetime not null,
		constraint pk$migration primary key clustered(migration_id asc)
	);
end
