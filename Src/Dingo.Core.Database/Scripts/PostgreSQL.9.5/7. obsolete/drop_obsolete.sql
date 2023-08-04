-- use dingo.drop_* functions to drop obsolete objects
select dingo.drop_routine('system__check_table_existence');
select dingo.drop_routine('system__get_migrations_status');
select dingo.drop_routine('system__register_migration');
