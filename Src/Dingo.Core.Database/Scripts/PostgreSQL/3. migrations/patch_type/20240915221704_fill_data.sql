-- up
do $$
begin
	set search_path to dingo;

	drop table if exists tt_source;
	create temp table tt_source as select patch_type_id, name from patch_type where 1 = 0;
	insert into tt_source (patch_type_id, name) values
		(1, 'system'),
		(2, 'user')
	;

	merge into patch_type as target
	using tt_source as source
		on source.patch_type_id = target.patch_type_id
	when not matched then
		insert (patch_type_id, name) values (source.patch_type_id, source.name)
	when matched and target.name <> source.name then
		update set name = source.name
	;

	delete from patch_type where patch_type_id not in (select patch_type_id from tt_source);
end $$;

-- down
do $$
begin
	set search_path to dingo;
	delete from patch_type where patch_type_id in (1, 2);
end $$;
