-- Drop function
select dingo.drop_routine('_register_migration', 'dingo');

-- Create function
create function dingo._register_migration(
	p_migration_path text,
	p_migration_hash varchar(256),
	p_patch_number integer
)
returns void as
$$ declare
	v_migration_exists bool;
	v_migration_id integer;
begin
	----------------------------------------------------------------
	select into v_migration_exists exists(
		select 1 from dingo.migration where migration_path = p_migration_path
	);
	----------------------------------------------------------------
	if (v_migration_exists is false) then
		insert into dingo.migration (migration_path) values (p_migration_path) returning migration_id into v_migration_id;
	else
		select into v_migration_id migration_id from dingo.migration where migration_path = p_migration_path limit 1;
	end if;
	----------------------------------------------------------------
	insert into dingo.patch_migration (migration_hash, migration_id, patch_number) values
		(p_migration_hash, v_migration_id, p_patch_number);
	----------------------------------------------------------------
end;
$$
language plpgsql;
