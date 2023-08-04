if not exists(select * from sys.schemas where name = 'dingo')
begin
    exec('create schema dingo')
end