create or replace function dingo.exists_type(
	p_type_name   varchar(512),
	p_schema_name varchar(128) = 'public'
)
returns boolean as
$$
declare v_type_exists boolean;
begin
	----------------------------------------------------------------
	select into v_type_exists exists (
		select from information_schema.user_defined_types
		where 1 = 1
			and information_schema.user_defined_types.user_defined_type_schema = p_schema_name
			and information_schema.user_defined_types.user_defined_type_name   = p_type_name
	);
	return v_type_exists;
	----------------------------------------------------------------
end;
$$
language plpgsql;
