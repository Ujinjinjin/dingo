-- Set comments on user defined functions
comment on function dingo._get_migrations_status is 'get status of given migrations';
comment on function dingo._register_migration is 'register migration with given name and hash';

comment on function dingo.drop_constraint is 'drops constraint with given name';
comment on function dingo.drop_default is 'drops default value on table column with given name';
comment on function dingo.drop_routine is 'drops all function reloads with given name at once';
comment on function dingo.drop_index is 'drops index with given name';
comment on function dingo.drop_type is 'drops user defined type with given name';

comment on function dingo.exists_column is 'checks if table column with given name exists';
comment on function dingo.exists_constraint is 'checks if constraint with given name exists';
comment on function dingo.exists_index is 'checks if index with given name exists';
comment on function dingo.exists_materialized_view is 'checks if materialized view with given name exists';
comment on function dingo.exists_table is 'checks if table with given name exists';
comment on function dingo.exists_type is 'checks if user defined type with given name exists';
comment on function dingo.exists_view is 'checks if view with given name exists';
