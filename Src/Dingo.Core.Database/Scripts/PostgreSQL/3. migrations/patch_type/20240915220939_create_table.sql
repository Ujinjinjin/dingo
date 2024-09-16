-- up
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_table('patch_type', 'dingo') is false) then
		create table patch_type(
			patch_type_id int not null,
			name varchar(8) not null
		);

		alter table patch_type add constraint pk$patch_type primary key (patch_type_id);
	end if;
end $$;

-- down
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_table('patch_type', 'dingo') is true) then
		drop table patch_type;
	end if;
end $$;
