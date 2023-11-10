-- up
if dingo.exists_table('patch', 'dingo') = 0
begin
	create table dingo.patch(
		patch_number int not null identity(1, 1),
		applied_at datetime null default null,
		reverted_at datetime null default null,
		reverted bit not null default 0
		constraint pk$patch primary key clustered(patch_number asc)
	);
end

-- down
if dingo.exists_table('patch', 'dingo') = 1
begin
	drop table dingo.patch;
end
