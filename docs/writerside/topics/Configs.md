# configs

`dingo` uses [`Trico`](https://github.com/Ujinjinjin/trico) as a configuration management tool and currently supports `yml`, `yaml` and `json` files and environment variables.

Use this section as a reference `dingo` configurations.

| configuration key         | required | default       |
|---------------------------|----------|---------------|
| `db.connection-string`    | ✓        | `null`        |
| `db.provider`             | ✕        | `PostgreSQL`  |
| `db.schema`               | ✕        | `dingo`       |
| `migration.delimiter`     | ✕        | `^--\s*down$` |
| `migration.wildcard`      | ✕        | `*.sql`       |
| `migration.down-required` | ✕        | `false`       |
| `log.level`               | ✕        | `Information` |

## db.connection-string



## db.provider

## db.schema

## migration.delimiter

## migration.wildcard

## migration.down-required

## log.level
