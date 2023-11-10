-- up
if dingo.exists_table('migration', 'dingo') = 0
begin
	create table dingo.migration(
		migration_id int not null identity(1, 1),
		migration_path nvarchar(512) not null,
		constraint pk$migration primary key clustered(migration_id asc)
	);
end

-- down
if dingo.exists_table('migration', 'dingo') = 1
begin
	drop table dingo.migration;
end
