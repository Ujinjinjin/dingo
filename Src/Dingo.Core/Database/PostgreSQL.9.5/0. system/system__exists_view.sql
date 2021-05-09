create or replace function system__exists_view(
	p_view_name   varchar(128),
	p_schema_name varchar(128) = 'public'
)
returns boolean as
$$
declare v_view_exists boolean;
begin
	----------------------------------------------------------------
	select into v_view_exists exists (
		select from information_schema.views
		where 1 = 1
			and information_schema.views.table_schema = p_schema_name
			and information_schema.views.table_name   = p_view_name
	);
	return v_view_exists;
	----------------------------------------------------------------
end;
$$
language plpgsql;
