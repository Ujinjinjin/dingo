create or replace function dingo.drop_routine(
	p_routine_name text,
	p_schema_name  varchar(128) = 'public'
)
returns text as
$$
declare
	i int;
	r record;
	v_parameters_number int;
	v_parameter_text text;
	v_dropped_count smallint := 0;
begin
	----------------------------------------------------------------
	for r in select
		information_schema.routines.routine_schema as schema_name,
		information_schema.routines.routine_name,
		pg_proc.proargtypes
	from information_schema.routines
	inner join pg_proc
		on pg_proc.proname = information_schema.routines.routine_name
	where 1 = 1
		and information_schema.routines.routine_schema = p_schema_name
		and information_schema.routines.routine_name   = p_routine_name
	loop
		----------------------------------------------------------------
		--for some reason array_upper is off by one for the oid vector type, hence the + 1
		v_parameters_number = array_upper(r.proargtypes, 1) + 1;
		----------------------------------------------------------------
		i = 0;
		v_parameter_text = '';
		----------------------------------------------------------------
		loop
			if i < v_parameters_number then
				if i > 0 then
					v_parameter_text = v_parameter_text || ', ';
				end if;
				v_parameter_text = v_parameter_text || (select pg_type.typname from pg_type where pg_type.oid = r.proargtypes[i]);
				i = i + 1;
			else
				exit;
			end if;
		end loop;
		----------------------------------------------------------------
		execute 'drop function ' || r.schema_name || '.' || r.routine_name || '(' || v_parameter_text || ');';
		v_dropped_count = v_dropped_count + 1;
		----------------------------------------------------------------
	end loop;
	----------------------------------------------------------------
	return 'dropped ' || v_dropped_count || ' functions';
	----------------------------------------------------------------
end;
$$
language plpgsql;
