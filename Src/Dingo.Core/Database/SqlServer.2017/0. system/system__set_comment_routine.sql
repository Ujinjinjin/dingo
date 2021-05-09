create or alter proc system__set_comment_routine(
	@p_routine_name varchar(max),
	@p_comment      nvarchar(512),
	@p_schema_name  varchar(128) = 'dbo'
)
as
begin
	----------------------------------------------------------------
	declare @v_routine_type varchar(16);
	----------------------------------------------------------------
	select top 1
		@v_routine_type = information_schema.routines.routine_type
	from information_schema.routines
	where 1 = 1
		and information_schema.routines.routine_schema = @p_schema_name
		and information_schema.routines.routine_name   = @p_routine_name
	----------------------------------------------------------------
	exec system__set_comment
		@p_comment     = @p_comment,
		@p_level1_name = @p_routine_name,
		@p_level1_type = @v_routine_type,
		@p_column_name = null,
		@p_schema_name = @p_schema_name
	;
	----------------------------------------------------------------
end;
go
