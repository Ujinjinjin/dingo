do $$begin
    if (select exists(select schema_name from information_schema.schemata where schema_name = 'dingo') is false) then
        create schema "dingo";
    end if;
end$$