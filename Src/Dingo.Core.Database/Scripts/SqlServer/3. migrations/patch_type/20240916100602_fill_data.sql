-- up
drop table if exists #tt_source;
select patch_type_id, name into #tt_source from dingo.patch_type where 1 = 0;
insert into #tt_source (patch_type_id, name) values
	(1, 'system'),
	(2, 'user')
;

merge into dingo.patch_type as target
using #tt_source as source
	on source.patch_type_id = target.patch_type_id
when not matched then
	insert (patch_type_id, name) values (source.patch_type_id, source.name)
when matched and target.name <> source.name then
	update set name = source.name
when not matched by source then delete
;

-- down
delete from dingo.patch_type where patch_type_id in (1, 2);