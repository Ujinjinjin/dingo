create or replace function system__exists_column(
	p_table_name  varchar(128),
	p_column_name varchar(128),
	p_schema_name varchar(128) = 'public'
)
returns boolean as
$$
declare v_column_exists boolean;
begin
	---------------	-------------------------------------------------
	select into v_column_exists exists (
		select from information_schema.columns 
		where 1 = 1
			and information_schema.columns.table_schema = p_schema_name
			and information_schema.columns.table_name   = p_table_name
			and information_schema.columns.column_name  = p_column_name
	);
	return v_column_exists;
	----------------------------------------------------------------
end;
$$
language plpgsql;