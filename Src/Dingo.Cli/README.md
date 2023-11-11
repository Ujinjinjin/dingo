# Release notes

## Summary

- long needed application refactoring in order to support easier extensibility and testability
- applying migrations in both directions: up and down
- support multiple configuration profiles
- performance improvements
- support more flexible configuration using:
  - configuration file (json, yaml)
  - environment variables
- configurations are stored in `.dingo` directory instead of single file
- rapid DB development tools. DB functions to check table, column and index existence; easy removal of DB objects and much more
- switched from `linq2db` to `dapper`
- `dingo` DB objects are stored in `dingo` schema instead of public / dbo
- specify paths that need to be re-applied in every patch
- improved migration validation
- improved unit and integration tests
- supported platforms: linux, macOS and windows
- supported DBMSs: PostgreSQL, SqlServer
- upgrade dependencies
- brand new [wiki](https://ujinjinjin.github.io/dingo/dingo.html) section
