-- up
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_table('patch', 'dingo') is false) then
		create table patch(
			patch_number serial not null,
			applied_at timestamp null default null,
			reverted_at timestamp null default null,
			reverted bool not null default false
		);

		alter table patch add constraint pk$patch primary key (patch_number);
	end if;
end $$;

-- down
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_table('patch', 'dingo') is true) then
		drop table patch;
	end if;
end $$;
