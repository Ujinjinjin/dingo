create or alter proc system__set_comment(
	@p_comment     nvarchar(512),
	@p_level1_name varchar(128),
	@p_level1_type varchar(128) = null,
	@p_column_name varchar(128) = null,
	@p_schema_name varchar(128) = 'dbo'
)
as
begin
	----------------------------------------------------------------
	if (object_id(@p_level1_name) is null)
	begin
		return;
	end
	----------------------------------------------------------------
	declare @c_property_name     varchar(128) = 'MS_Description';
	declare @c_level0type_schema varchar(16) = 'SCHEMA';
	declare @c_level1type_table  varchar(16) = 'TABLE';
	declare @c_level2type_column varchar(16) = 'COLUMN';
	----------------------------------------------------------------
	declare @v_level2type varchar(16) = null;
	declare @v_major_id   int = object_id(@p_level1_name);
	declare @v_minor_id   int = 0;
	----------------------------------------------------------------
	set @p_level1_type = isnull(@p_level1_type, @c_level1type_table);
	----------------------------------------------------------------
	if @p_column_name is not null
	begin
		----------------------------------------------------------------
		set @v_level2type = @c_level2type_column;
		----------------------------------------------------------------
		select @v_minor_id = columns.column_id
		from sys.objects
		inner join sys.columns
			on columns.object_id  = objects.object_id
			and columns.name      = @p_column_name
		where 1 = 1
			and objects.object_id = @v_major_id
		;
		----------------------------------------------------------------
	end;
	----------------------------------------------------------------
	if not exists(
		select 1
		from sys.extended_properties
		where 1 = 1
			and extended_properties.name     = @c_property_name
			and extended_properties.major_id = @v_major_id
			and extended_properties.minor_id = @v_minor_id
	)
	begin
		----------------------------------------------------------------
		execute sp_addextendedproperty
			@name       = @c_property_name,
			@value      = @p_comment,
			@level0type = @c_level0type_schema,
			@level0name = @p_schema_name,
			@level1type = @p_level1_type,
			@level1name = @p_level1_name,
			@level2type = @v_level2type,
			@level2name = @p_column_name
		;
		----------------------------------------------------------------
	end
	else
	begin
		----------------------------------------------------------------
		execute sp_updateextendedproperty
			@name       = @c_property_name,
			@value      = @p_comment,
			@level0type = @c_level0type_schema,
			@level0name = @p_schema_name,
			@level1type = @p_level1_type,
			@level1name = @p_level1_name,
			@level2type = @v_level2type,
			@level2name = @p_column_name
		;
		----------------------------------------------------------------
	end;
end;
go
