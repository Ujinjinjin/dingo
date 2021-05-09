create or alter function system__exists_materialized_view(
	@p_mat_view_name varchar(128),
	@p_schema_name   varchar(128) = 'dbo'
)
returns bit
as
begin
	----------------------------------------------------------------
	declare @v_mat_view_exists bit = 0;
	----------------------------------------------------------------
	if exists(
		select 1
		from sys.views
		inner join sys.indexes
			on sys.indexes.object_id       = sys.views.object_id
			and sys.indexes.index_id       = 1
			and sys.indexes.ignore_dup_key = 0
		inner join sys.sql_modules
			on sys.sql_modules.object_id   = sys.views.object_id
		where 1 = 1
			and sys.views.schema_id = schema_id(@p_schema_name)
			and sys.views.name      = @p_mat_view_name
	)
	begin
		set @v_mat_view_exists = 1;
	end;
	----------------------------------------------------------------
	return @v_mat_view_exists;
	----------------------------------------------------------------
end;
