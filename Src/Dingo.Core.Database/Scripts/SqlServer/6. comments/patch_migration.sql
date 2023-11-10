-- Set comments
exec dingo.set_comment_table 'patch_migration', 'applied migration registry', 'dingo';
exec dingo.set_comment_table_column 'patch_migration', 'migration_hash', 'migration hash at time of application', 'dingo';
exec dingo.set_comment_table_column 'patch_migration', 'migration_id', 'unique migration ID', 'dingo';
exec dingo.set_comment_table_column 'patch_migration', 'patch_number', 'patch that includes applied migration', 'dingo';
