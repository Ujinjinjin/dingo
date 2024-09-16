-- up
do $$
begin
	set search_path to dingo;

	if (select
		dingo.exists_table('patch', 'dingo') is true and
		dingo.exists_column('patch', 'type', 'dingo') is false
	) then
		alter table patch add "type" int not null default 2; -- patch_type = user
		perform dingo.drop_default('patch', 'type', 'dingo');
	end if;
end $$;

-- down
do $$
begin
	set search_path to dingo;

	if (select
		dingo.exists_table('patch', 'dingo') is true and
		dingo.exists_column('patch', 'type', 'dingo') is true
	) then
		alter table patch drop column "type";
	end if;
end $$;
