-- up
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_index('patch', 'i$patch$reverted', 'dingo') is false) then
		create index i$patch$reverted on dingo.patch (reverted);
	end if;
end $$;

-- down
do $$
begin
	set search_path to dingo;

	if (select dingo.exists_index('patch', 'i$patch$reverted', 'dingo') is true) then
		select dingo.drop_index('patch', 'i$patch$reverted', 'dingo');
	end if;
end $$;
