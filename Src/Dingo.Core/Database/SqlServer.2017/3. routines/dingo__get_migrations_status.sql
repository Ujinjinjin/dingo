create or alter procedure dingo__get_migrations_status(
	@pti_migration_info_input t_migration_info_input readonly
)
as
begin
	----------------------------------------------------------------
	declare @c_unreal_char char = -1;
	----------------------------------------------------------------
	select
		outer_table.migration_path,
		outer_table.new_hash,
		outer_table.old_hash,
		outer_table.is_outdated,
		outer_table.date_updated
	from @pti_migration_info_input as inner_table
	cross apply (
		select top(1)
			t1_input.migration_path,
			t1_input.migration_hash as new_hash,
			dingo_migration.migration_hash as old_hash,
			iif(isnull(t1_input.migration_hash, @c_unreal_char) = isnull(dingo_migration.migration_hash, @c_unreal_char), 0, 1) as is_outdated,
			dingo_migration.date_updated
		from @pti_migration_info_input as t1_input
		left outer join dingo_migration
			on t1_input.migration_path = dingo_migration.migration_path
		where 1 = 1
			and t1_input.migration_path = inner_table.migration_path
		order by dingo_migration.date_updated desc
	) as outer_table
	;
	----------------------------------------------------------------
end;
