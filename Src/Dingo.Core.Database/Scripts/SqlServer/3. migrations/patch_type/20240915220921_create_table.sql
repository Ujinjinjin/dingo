-- up
if dingo.exists_table('patch_type', 'dingo') = 0
begin
	create table dingo.patch_type(
		patch_type_id int not null,
		name varchar(8) not null
		constraint pk$patch_type primary key clustered(patch_type_id asc)
	);
end

-- down
if dingo.exists_table('patch_type', 'dingo') = 1
begin
	drop table dingo.patch_type;
end
