create or replace function system__exists_table(
	p_table_name  varchar(128),
	p_schema_name varchar(128) = 'public'
)
returns boolean as
$$
declare v_table_exists boolean;
begin
	----------------------------------------------------------------
	select into v_table_exists exists (
		select from information_schema.tables
		where 1 = 1
			and information_schema.tables.table_schema = p_schema_name
			and information_schema.tables.table_name   = p_table_name
	);
	return v_table_exists;
	----------------------------------------------------------------
end;
$$
language plpgsql;
