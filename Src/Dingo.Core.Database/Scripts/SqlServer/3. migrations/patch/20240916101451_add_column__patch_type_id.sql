-- up
if dingo.exists_table('patch', 'dingo') = 1 and dingo.exists_column('patch', 'type', 'dingo') = 0
begin
	alter table dingo.patch add [type] int not null default 2; -- patch_type = user

	exec dingo.drop_default 'patch', 'type', 'dingo';
end

-- down
if dingo.exists_table('patch', 'dingo') = 1 and dingo.exists_column('patch', 'type', 'dingo') = 1
begin
	alter table dingo.patch drop column [type];
end
