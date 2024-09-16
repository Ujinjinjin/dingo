-- up
insert into dingo.patch_type (patch_type_id, name) values
	(1, 'system'),
	(2, 'user');

-- down
delete from dingo.patch_type where patch_type_id in (1, 2);