-- Set comments on user defined routines
exec dingo.set_comment_routine '_get_migrations_status', 'get status of given migrations', 'dingo';
exec dingo.set_comment_routine '_register_migration', 'register migration with given name and hash', 'dingo';

exec dingo.set_comment_routine 'drop_constraint', 'drops constraint with given name', 'dingo';
exec dingo.set_comment_routine 'drop_default', 'drops default value on table column with given name', 'dingo';
exec dingo.set_comment_routine 'drop_index', 'drops index with given name', 'dingo';
exec dingo.set_comment_routine 'drop_routine', 'drops routine with given name, be it function or procedure', 'dingo';
exec dingo.set_comment_routine 'drop_type', 'drops user defined type with given name', 'dingo';

exec dingo.set_comment_routine 'exists_column', 'checks if table column with given name exists', 'dingo';
exec dingo.set_comment_routine 'exists_constraint', 'checks if constraint with given name exists', 'dingo';
exec dingo.set_comment_routine 'exists_index', 'checks if index with given name exists', 'dingo';
exec dingo.set_comment_routine 'exists_materialized_view', 'checks if materialized view with given name exists', 'dingo';
exec dingo.set_comment_routine 'exists_table', 'checks if table with given name exists', 'dingo';
exec dingo.set_comment_routine 'exists_type', 'checks if user defined type with given name exists', 'dingo';
exec dingo.set_comment_routine 'exists_view', 'checks if view with given name exists', 'dingo';

exec dingo.set_comment_routine 'set_comment', 'sets given comment on specified object', 'dingo';
exec dingo.set_comment_routine 'set_comment_routine', 'sets given comment on user defined routine', 'dingo';
exec dingo.set_comment_routine 'set_comment_table', 'sets given comment on table', 'dingo';
exec dingo.set_comment_routine 'set_comment_table_column', 'sets given comment on table column', 'dingo';
exec dingo.set_comment_routine 'set_comment_type', 'sets given comment on user defined type', 'dingo';
exec dingo.set_comment_routine 'set_comment_type_column', 'sets given comment on type column', 'dingo';
exec dingo.set_comment_routine 'set_comment_view', 'sets given comment on view', 'dingo';
exec dingo.set_comment_routine 'set_comment_view_column', 'sets given comment on view column', 'dingo';
