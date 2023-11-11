# dingo

`dingo` is a framework-agnostic lightweight cross-platform database migration tool that will keep your database schema in sync across multiple developers and deployment environments.

It's a command line tool that can be used on any project regardless of the programming language used to create it, be it Python, Go, C#, Java, Ruby, PHP or something else. This tool fits especially well with microservice architecture and services written in different languages, allowing you to have consistent and unified database migration process.

## Key features

- Supports Postgres and SQL Server
- Migrations are timestamp-versioned
- Migrations are written using plain `.sql`
- Project configs can be stored as `.yml`, `.json` or environment variables
- Use different configuration profiles for different environments
- Provides tools for rapid database development
- Apply and revert database migrations

## Summary

|               |                                                                                                                                                                                                       |
|---------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Build Status  | [![Build Status](https://dev.azure.com/ujinjinjin/Dingo/_apis/build/status%2FDingo.%20CLI?branchName=master)](https://dev.azure.com/ujinjinjin/Dingo/_build/latest?definitionId=15&branchName=master) |
| Unit Tests    | [![Azure DevOps tests](https://img.shields.io/azure-devops/tests/ujinjinjin/Dingo/12?label=Unit%20tests)](https://dev.azure.com/ujinjinjin/Dingo/_build/latest?definitionId=12&branchName=master)     |
| Test Coverage | ![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/ujinjinjin/dingo/20)                                                                                                            |
| Version       | ![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/ujinjinjin/dingo)                                                                                                           |
| Downloads     | ![GitHub all releases](https://img.shields.io/github/downloads/ujinjinjin/dingo/total)                                                                                                                |
| License       | [![GitHub](https://img.shields.io/github/license/ujinjinjin/dingo)](https://github.com/Ujinjinjin/dingo/blob/master/LICENSE)                                                                          |
| Docs          | [![Static Badge](https://img.shields.io/badge/docs-wiki-blue)](https://ujinjinjin.github.io/dingo/dingo.html)                                                                                         |

## Installation

### Linux

To install `dingo` on Linux run following commands:

```shell
curl -s https://api.github.com/repos/ujinjinjin/dingo/releases/latest \
    | grep "browser_download_url.*deb" \
    | cut -d '"' -f 4 \
    | wget -O dingo.deb -qi -
sudo dpkg --install dingo.deb
```
Then add `/usr/share/dingo` to the `$PATH` environment variable. Run following command to validate successful installation:

```shell
dingo --version
```

To uninstall `dingo` use:

```shell
sudo dpkg --remove dingo
```

### macOS

To install `dingo` on macOS run following commands:

```shell
curl -s https://api.github.com/repos/ujinjinjin/dingo/releases/latest \
    | grep "browser_download_url.*pkg" \
    | cut -d '"' -f 4 \
    | xargs -I packageUrl curl packageUrl -L -s -o dingo.pkg
installer -pkg dingo.pkg -target CurrentUserHomeDirectory
```

## Getting started

Let's apply our first migrations on a PostgreSQL database. Firstly, create project folder:

```shell
mkdir user-service
cd user-service
```

Then initialize `dingo` configuration profile using command below:

```shell
dingo init
```

The command above will create `user-service/.dingo/config.yml` file. Open it and add content from the snippet below and replace `HOST`, `DB_NAME`, `USERNAME` and `PWD` with relevant values:

```yaml
db:
  connection-string: Server=HOST;Database=DB_NAME;User Id=USERNAME;Password=PWD;
  provider: PostgreSQL
```

Now let's create our first migration at `user-service/migrations/tables/users`:

```shell
dingo new -n create_table -p user-service/migrations/tables/users
```

The command will create a file `user-service/migrations/tables/users/YYYYMMDDmmHHss_create_table.sql`, where `YYYYMMDDmmHHss` is a timestamp. Let's open it and fill out with migration logic:

```postgresql
-- up
create table "user" (
  user_id serial not null,
  username text not null
);

-- down
drop table "user";
```

Now let's apply migration using following command:

```shell
dingo up -p user-service/migrations
```

Done! You can check your DB, migration is applied and everything should be up-to-date.

If you wish to rollback last applied patch (execute down part of migrations), you can do that using:
```shell
dingo down -p user-service/migrations
```

For additional information on usage refer to [wiki](https://ujinjinjin.github.io/dingo/dingo.html) or run:

```shell
dingo --help
```
