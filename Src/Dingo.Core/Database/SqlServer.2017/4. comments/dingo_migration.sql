-- Set comments
exec system__set_comment_table 'dingo_migration', 'applied migration registry', default;
exec system__set_comment_table_column 'dingo_migration', 'date_updated', 'timestamp when migration was applied', default;
exec system__set_comment_table_column 'dingo_migration', 'dingo_migration_id', 'unique migration ID', default;
exec system__set_comment_table_column 'dingo_migration', 'migration_hash', 'hash of migration file', default;
exec system__set_comment_table_column 'dingo_migration', 'migration_path', 'relative path to migration file', default;