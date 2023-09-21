create or alter procedure dingo._get_migrations_status(
	@pti_migration_info_input t_migration_info_input readonly
)
as
begin
	----------------------------------------------------------------
	declare @c_unreal_char char = -1;
	----------------------------------------------------------------
	select
		outer_table.migration_hash as [hash],
		outer_table.hash_matches
	from @pti_migration_info_input as inner_table
	cross apply (
		select top(1)
			t1_input.migration_hash,
			iif(isnull(t1_input.migration_hash, @c_unreal_char) = isnull(migration.migration_hash, @c_unreal_char), 1, 0) as hash_matches
		from @pti_migration_info_input as t1_input
		left outer join dingo.migration as migration
			on t1_input.migration_path = migration.migration_path
		where 1 = 1
			and t1_input.migration_path = inner_table.migration_path
		order by migration.date_updated desc
	) as outer_table
	;
	----------------------------------------------------------------
end;
