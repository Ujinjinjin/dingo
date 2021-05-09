-- use system__drop_* functions to drop obsolete objects
select system__drop_routine('system__check_table_existence');
select system__drop_routine('system__get_migrations_status');
select system__drop_routine('system__register_migration');
