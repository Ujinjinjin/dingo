-- up
if dingo.exists_index('migration', 'ui$migration$migration_path', 'dingo') = 0
begin
	create unique index ui$migration$migration_path on dingo.migration (migration_path);
end

-- down
if dingo.exists_index('migration', 'ui$migration$migration_path', 'dingo') = 1
begin
	exec dingo.drop_index 'migration', 'ui$migration$migration_path', 'dingo'
end
