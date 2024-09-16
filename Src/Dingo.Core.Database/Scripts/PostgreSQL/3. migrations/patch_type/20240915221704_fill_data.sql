-- up
do $$
begin
	set search_path to dingo;
	insert into patch_type (patch_type_id, name) values
		(1, 'system'),
		(2, 'user');
end $$;

-- down
do $$
begin
	set search_path to dingo;
	delete from patch_type where patch_type_id in (1, 2);
end $$;
