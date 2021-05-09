-- use system__drop_* functions to drop obsolete objects
exec system__drop_routine 'system__check_table_existence', default;
exec system__drop_routine 'system__get_migrations_status', default;
exec system__drop_routine 'system__register_migration', default;
