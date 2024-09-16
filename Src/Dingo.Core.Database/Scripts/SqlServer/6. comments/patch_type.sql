-- Set comments
exec dingo.set_comment_table 'patch_type', 'enumeration of patch types', 'dingo';
exec dingo.set_comment_table_column 'patch_type', 'patch_type_id', 'unique patch type identifier', 'dingo';
exec dingo.set_comment_table_column 'patch_type', 'name', 'short descriptive patch type name', 'dingo';
