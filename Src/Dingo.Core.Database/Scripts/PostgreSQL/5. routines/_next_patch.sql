-- Drop function
select dingo.drop_routine('_next_patch', 'dingo');

-- Create function
create function dingo._next_patch(
	p_patch_type integer
)
returns integer as
$$ declare
	v_patch_number integer;
	v_last_patch_empty bool;
begin
	----------------------------------------------------------------
	select into v_patch_number (
		select patch.patch_number
		from dingo.patch as patch
		order by patch.patch_number desc
		limit 1
	);
	----------------------------------------------------------------
	-- if there are no patches in the table, create one
	if (v_patch_number is null) then
		insert into dingo.patch (applied_at, reverted_at, reverted, "type") values
			(default, default, default, p_patch_type)
		returning patch_number into v_patch_number;
	end if;
	----------------------------------------------------------------
	select into v_last_patch_empty not exists(
		select 1
		from dingo.patch_migration as patch_migration
		where patch_migration.patch_number = v_patch_number
	);
	----------------------------------------------------------------
	-- create new patch only if the last one is not empty
	if (v_last_patch_empty is false) then
		insert into dingo.patch (applied_at, reverted_at, reverted, "type") values
			(default, default, default, p_patch_type)
		returning patch_number into v_patch_number;
	else -- if last patch is empty, update its type
		update dingo.patch set "type" = p_patch_type where patch_number = v_patch_number;
	end if;
	----------------------------------------------------------------
	return v_patch_number;
	----------------------------------------------------------------
end;
$$
language plpgsql;
