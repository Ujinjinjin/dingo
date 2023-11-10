-- Set comments
exec dingo.set_comment_table 'migration', 'applied migration registry', 'dingo';
exec dingo.set_comment_table_column 'migration', 'migration_id', 'unique migration ID', 'dingo';
exec dingo.set_comment_table_column 'migration', 'migration_path', 'relative path to migration file', 'dingo';