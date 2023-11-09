# database tools

During the first migration `dingo` installs a set of stored procedures and functions aimed to support rapid database development.

|                          | PostgreSQL | SqlServer |
|--------------------------|------------|-----------|
| drop_constraint          | ✓          | ✓         |
| drop_default             | ✓          | ✓         |
| drop_index               | ✓          | ✓         |
| drop_routine             | ✓          | ✓         |
| drop_type                | ✓          | ✓         |
| exists_column            | ✓          | ✓         |
| exists_constraint        | ✓          | ✓         |
| exists_index             | ✓          | ✓         |
| exists_materialized_view | ✓          | ✓         |
| exists_table             | ✓          | ✓         |
| exists_type              | ✓          | ✓         |
| exists_view              | ✓          | ✓         |
| set_comment              | ✕          | ✓         |
| set_comment_routine      | ✕          | ✓         |
| set_comment_table        | ✕          | ✓         |
| set_comment_table_column | ✕          | ✓         |
| set_comment_type         | ✕          | ✓         |
| set_comment_type_column  | ✕          | ✓         |
| set_comment_view         | ✕          | ✓         |
| set_comment_view_column  | ✕          | ✓         |
{style="both"}

> If you are using Sql Server, all routine parameter names must have preceding `@` character. E.g.: `@p_table_name`
{style="info"}
 
> `dingo` doesn't provide `set_comment` functions for PostgreSQL, since it can be done using `comment on (table | column | function | ...)` supported by Postgres itself
{style="note"}

## drop_constraint

Drop constraint on the table by its name

**Parameters**:

|                   | type   | required | default           |
|-------------------|--------|----------|-------------------|
| p_table_name      | string | ✓        | `null`            |
| p_constraint_name | string | ✓        | `null`            |
| p_schema_name     | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                select dingo.drop_constraint('table', 'constraint', 'schema');
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.drop_constraint 'table', 'constraint', 'schema';
        </code-block>
    </tab>
</tabs>

## drop_default

Drop `default` constraint on the table column

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_table_name  | string | ✓        | `null`            |
| p_column_name | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                select dingo.drop_default('table', 'column', 'schema');
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.drop_default 'table', 'column', 'schema';
        </code-block>
    </tab>
</tabs>

## drop_index

Drop index on the table by its name

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_table_name  | string | ✓        | `null`            |
| p_index_name  | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                select dingo.drop_index('table', 'index', 'schema');
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.drop_index 'table', 'index', 'schema';
        </code-block>
    </tab>
</tabs>

## drop_routine

Drop user-defined routine by its name

**Parameters**:

|                | type   | required | default           |
|----------------|--------|----------|-------------------|
| p_routine_name | string | ✓        | `null`            |
| p_schema_name  | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                select dingo.drop_routine('routine', 'schema');
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.drop_routine 'routine', 'schema';
        </code-block>
    </tab>
</tabs>

## drop_type

Drop user-defined type by its name

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_type_name   | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                select dingo.drop_type('type', 'schema');
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.drop_type 'type', 'schema';
        </code-block>
    </tab>
</tabs>

## exists_column

Check if column with given name exists on the table

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_table_name  | string | ✓        | `null`            |
| p_column_name | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                if (select dingo.exists_column('table', 'column', 'schema') is false) then
                    -- do something
                end if;
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            if dingo.exists_table('table', 'column', 'schema') = 0
            begin
                -- do something
            end
        </code-block>
    </tab>
</tabs>

## exists_constraint

Check if constraint with given name exists on the table

**Parameters**:

|                   | type   | required | default           |
|-------------------|--------|----------|-------------------|
| p_table_name      | string | ✓        | `null`            |
| p_constraint_name | string | ✓        | `null`            |
| p_schema_name     | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                if (select dingo.exists_constraint('table', 'constraint', 'schema') is false) then
                    -- do something
                end if;
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            if dingo.exists_constraint('table', 'constraint', 'schema') = 0
            begin
                -- do something
            end
        </code-block>
    </tab>
</tabs>

## exists_index

Check if index with given name exists on the table

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_table_name  | string | ✓        | `null`            |
| p_index_name  | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                if (select dingo.exists_index('table', 'index', 'schema') is false) then
                    -- do something
                end if;
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            if dingo.exists_index('table', 'index', 'schema') = 0
            begin
                -- do something
            end
        </code-block>
    </tab>
</tabs>

## exists_materialized_view

Check if materialized view with given name exists

**Parameters**:

|                 | type   | required | default           |
|-----------------|--------|----------|-------------------|
| p_mat_view_name | string | ✓        | `null`            |
| p_schema_name   | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                if (select dingo.exists_materialized_view('mat_view', 'schema') is false) then
                    -- do something
                end if;
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            if dingo.exists_materialized_view('mat_view', 'schema') = 0
            begin
                -- do something
            end
        </code-block>
    </tab>
</tabs>

## exists_table

Check if table with given name exists

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_table_name  | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                if (select dingo.exists_table('table', 'schema') is false) then
                    -- do something
                end if;
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            if dingo.exists_table('table', 'schema') = 0
            begin
                -- do something
            end
        </code-block>
    </tab>
</tabs>

## exists_type

Check if user-defined type with given name exists

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_type_name   | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                if (select dingo.exists_type('type', 'schema') is false) then
                    -- do something
                end if;
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            if dingo.exists_type('type', 'schema') = 0
            begin
                -- do something
            end
        </code-block>
    </tab>
</tabs>

## exists_view

Check if user-defined view with given name exists

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_view_name   | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            do $$
            begin
                if (select dingo.exists_view('view', 'schema') is false) then
                    -- do something
                end if;
            end $$;
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            if dingo.exists_view('view', 'schema') = 0
            begin
                -- do something
            end
        </code-block>
    </tab>
</tabs>

## set_comment_routine

Set comment on a user-defined routine

**Parameters**:

|                | type   | required | default           |
|----------------|--------|----------|-------------------|
| p_routine_name | string | ✓        | `null`            |
| p_comment      | string | ✓        | `null`            |
| p_schema_name  | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            -- not supported
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.set_comment_routine 'routine', 'comment';
        </code-block>
    </tab>
</tabs>

## set_comment_table

Set comment on a table

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_table_name  | string | ✓        | `null`            |
| p_comment     | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            -- not supported
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.set_comment_table 'table', 'comment';
        </code-block>
    </tab>
</tabs>

## set_comment_table_column

Set comment on a table column

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_table_name  | string | ✓        | `null`            |
| p_column_name | string | ✓        | `null`            |
| p_comment     | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            -- not supported
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.set_comment_table_column 'table', 'column', 'comment';
        </code-block>
    </tab>
</tabs>

## set_comment_type

Set comment on a user-defined type

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_type_name   | string | ✓        | `null`            |
| p_comment     | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            -- not supported
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.set_comment_type 'type', 'comment';
        </code-block>
    </tab>
</tabs>

## set_comment_type_column

Set comment on a type column

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_type_name   | string | ✓        | `null`            |
| p_column_name | string | ✓        | `null`            |
| p_comment     | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            -- not supported
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.set_comment_type_column 'type', 'column', 'comment';
        </code-block>
    </tab>
</tabs>

## set_comment_view

Set comment on a view

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_view_name   | string | ✓        | `null`            |
| p_comment     | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            -- not supported
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.set_comment_view 'view', 'comment';
        </code-block>
    </tab>
</tabs>

## set_comment_view_column

Set comment on a view column

**Parameters**:

|               | type   | required | default           |
|---------------|--------|----------|-------------------|
| p_view_name   | string | ✓        | `null`            |
| p_column_name | string | ✓        | `null`            |
| p_comment     | string | ✓        | `null`            |
| p_schema_name | string | ✕        | `public` \| `dbo` |
{style="both"}

**Usage**:

<tabs group="db-provider">
    <tab title="PostgreSQL" group-key="db-postgres">
        <code-block lang="sql">
            -- not supported
        </code-block>
    </tab>
    <tab title="Sql Server" group-key="db-sqlserver">
        <code-block lang="sql">
            exec dingo.set_comment_view_column 'view', 'column', 'comment';
        </code-block>
    </tab>
</tabs>
