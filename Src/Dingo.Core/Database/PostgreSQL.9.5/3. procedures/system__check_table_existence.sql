create or replace function system__check_table_existence(
	p_table_schema varchar(128),
	p_table_name varchar(128)
)
returns boolean as
$table_exists$
declare v_table_exists boolean;
begin
	select into v_table_exists exists (
		select from information_schema.tables
		where 1 = 1
			and information_schema.tables.table_schema = p_table_schema
			and information_schema.tables.table_name   = p_table_name
	);
	return v_table_exists;
end;
$table_exists$
language plpgsql;
