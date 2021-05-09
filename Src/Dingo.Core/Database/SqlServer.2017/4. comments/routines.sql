-- Set comments on user defined routines
exec system__set_comment_routine 'dingo__get_migrations_status', 'get status of given migrations', default;
exec system__set_comment_routine 'dingo__register_migration', 'register migration with given name and hash', default;

exec system__set_comment_routine 'system__drop_constraint', 'drops constraint with given name', default;
exec system__set_comment_routine 'system__drop_default', 'drops default value on table column with given name', default;
exec system__set_comment_routine 'system__drop_index', 'drops index with given name', default;
exec system__set_comment_routine 'system__drop_routine', 'drops routine with given name, be it function or procedure', default;
exec system__set_comment_routine 'system__drop_type', 'drops user defined type with given name', default;

exec system__set_comment_routine 'system__exists_column', 'checks if table column with given name exists', default;
exec system__set_comment_routine 'system__exists_constraint', 'checks if constraint with given name exists', default;
exec system__set_comment_routine 'system__exists_index', 'checks if index with given name exists', default;
exec system__set_comment_routine 'system__exists_materialized_view', 'checks if materialized view with given name exists', default;
exec system__set_comment_routine 'system__exists_table', 'checks if table with given name exists', default;
exec system__set_comment_routine 'system__exists_type', 'checks if user defined type with given name exists', default;
exec system__set_comment_routine 'system__exists_view', 'checks if view with given name exists', default;

exec system__set_comment_routine 'system__set_comment', 'sets given comment on specified object', default;
exec system__set_comment_routine 'system__set_comment_routine', 'sets given comment on user defined routine', default;
exec system__set_comment_routine 'system__set_comment_table', 'sets given comment on table', default;
exec system__set_comment_routine 'system__set_comment_table_column', 'sets given comment on table column', default;
exec system__set_comment_routine 'system__set_comment_type', 'sets given comment on user defined type', default;
exec system__set_comment_routine 'system__set_comment_type_column', 'sets given comment on type column', default;
exec system__set_comment_routine 'system__set_comment_view', 'sets given comment on view', default;
exec system__set_comment_routine 'system__set_comment_view_column', 'sets given comment on view column', default;
