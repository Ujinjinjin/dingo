# dingo

Dingo is platform agnostic database migration tool, helping to automate database schema updates.

Because it uses plain `.sql` scripts for writing database migrations, it can be used alongside with applications written in any known programming language.

## Key features

- Supports PostgreSQL and SQL Server
- Migrations are timestamp-versioned
- Migrations are written using plain `.sql`
- Project configs can be stored both in `.yml` and `.json`
- Config file can be specified as command line argument

## Summary

|   |   |
|---|---|
| Build Status | [![Build Status](https://dev.azure.com/ujinjinjin/Dingo/_apis/build/status/Ujinjinjin.dingo?branchName=master)](https://dev.azure.com/ujinjinjin/Dingo/_build/latest?definitionId=12&branchName=master) |
| Unit Tests | [![Azure DevOps tests](https://img.shields.io/azure-devops/tests/ujinjinjin/Dingo/12?label=Unit%20tests)](https://dev.azure.com/ujinjinjin/Dingo/_build/latest?definitionId=12&branchName=master) |
| Test Coverage | [![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/ujinjinjin/dingo/12?label=Code%20coverage)](https://dev.azure.com/ujinjinjin/Dingo/_build/latest?definitionId=12&branchName=master) |
| Version | ![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/ujinjinjin/dingo) |
| Downloads | ![GitHub all releases](https://img.shields.io/github/downloads/ujinjinjin/dingo/total) |
| License | [![GitHub](https://img.shields.io/github/license/ujinjinjin/dingo)](https://github.com/Ujinjinjin/dingo/blob/master/LICENSE) |

## Installation

### Ubuntu 20.04

To install `dingo` on Ubuntu-20.04 run following commands:

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

## Usage

To collect and apply database migrations you should use following command:

```shell
dingo migrations run -m SQL_MIGRATIONS_PATH -c CONFIG_PATH
```

specifying:
- SQL_MIGRATIONS_PATH - root path to SQL migrations;
- CONFIG_PATH - path to dingo configuration file;
  
or

```shell
dingo migrations run -m SQL_MIGRATIONS_PATH \
                       --connectionString CONNECTION_STRING \
                       --providerName PROVIDER_NAME \
                       --migrationSchema MIGRATIONS_SCHEMA \
                       --migrationTable MIGRATIONS_TABLE
```

specifying:
- SQL_MIGRATIONS_PATH - root path to SQL migrations;
- CONNECTION_STRING - database connection string;
- PROVIDER_NAME - database provider name;
- MIGRATIONS_SCHEMA - database schema for you migrations;
- MIGRATIONS_TABLE - database table, where all migrations are stored;

For additional information on usage refer to [wiki](https://github.com/Ujinjinjin/dingo/wiki) or run:

```shell
dingo --help
```
