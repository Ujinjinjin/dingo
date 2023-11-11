# configs

`dingo` uses [`Trico`](https://github.com/Ujinjinjin/trico) as a configuration management tool and currently supports `yml`, `yaml` and `json` files and environment variables.

Use this section as a reference `dingo` configurations.

| configuration key         | required | default       |
|---------------------------|----------|---------------|
| `db.connection-string`    | ✓        | `null`        |
| `db.provider`             | ✕        | `PostgreSQL`  |
| `migration.delimiter`     | ✕        | `^--\s*down$` |
| `migration.wildcard`      | ✕        | `*.sql`       |
| `migration.down-required` | ✕        | `false`       |
| `log.level`               | ✕        | `Information` |

## db.connection-string

Database connection string in the following format: `Server=SERVER_HOST;Database=DB__NAME;User Id=USERNAME;Password=PWD;`

<tabs group="config-type">
    <tab title="yaml" group-key="config-yaml">
        <code-block lang="yaml">
            db:
              connection-string: Server=127.0.0.1;Database=dingo;User Id=dingo_usr;Password=VeryStr0ngPwd#;
        </code-block>
    </tab>
    <tab title="json" group-key="config-json">
        <code-block lang="json">
            {
                "db": {
                    "connection-string": "Server=127.0.0.1;Database=dingo;User Id=dingo_usr;Password=VeryStr0ngPwd#;"
                }
            }
        </code-block>
    </tab>
    <tab title="env" group-key="config-env">
        <code-block lang="shell">
            export DINGO_DB__CONNECTION_STRING="Server=127.0.0.1;Database=dingo;User Id=dingo_usr;Password=VeryStr0ngPwd#;"
        </code-block>
    </tab>
</tabs>

## db.provider

Database provider of the target DB where migrations will be applied

<tabs group="config-type">
    <tab title="yaml" group-key="config-yaml">
        <code-block lang="yaml">
            db:
              provider: PostgreSQL
        </code-block>
    </tab>
    <tab title="json" group-key="config-json">
        <code-block lang="json">
            {
                "db": {
                    "provider": "PostgreSQL"
                }
            }
        </code-block>
    </tab>
    <tab title="env" group-key="config-env">
        <code-block lang="shell">
            export DINGO_DB__PROVIDER=PostgreSQL
        </code-block>
    </tab>
</tabs>


> **Allowed values**: `PostgreSQL` | `Postgres` | `SqlServer`
{style="info"}

## migration.delimiter

Regular expression to specify migration file line that separates `up` and `down` parts of a migration.

<tabs group="config-type">
    <tab title="yaml" group-key="config-yaml">
        <code-block lang="yaml">
            migration:
              delimiter: '^--\s*down$'
        </code-block>
    </tab>
    <tab title="json" group-key="config-json">
        <code-block lang="json">
            {
                "migration": {
                    "delimiter": "^--\s*down$"
                }
            }
        </code-block>
    </tab>
    <tab title="env" group-key="config-env">
        <code-block lang="shell">
            export DINGO_MIGRATION__DELIMITER="^--\s*down$"
        </code-block>
    </tab>
</tabs>

## migration.wildcard

Wildcard used to search migration files in the specified directory

<tabs group="config-type">
    <tab title="yaml" group-key="config-yaml">
        <code-block lang="yaml">
            migration:
              wildcard: *.sql
        </code-block>
    </tab>
    <tab title="json" group-key="config-json">
        <code-block lang="json">
            {
                "migration": {
                    "wildcard": "*.sql"
                }
            }
        </code-block>
    </tab>
    <tab title="env" group-key="config-env">
        <code-block lang="shell">
            export DINGO_MIGRATION__WILDCARD="*.sql"
        </code-block>
    </tab>
</tabs>

## migration.down-required

Specifies if `down` part of migrations is required. If `true` - applying migrations will fail if any of migration files in the specified directory lack the `down` part.

<tabs group="config-type">
    <tab title="yaml" group-key="config-yaml">
        <code-block lang="yaml">
            migration:
              down-required: false
        </code-block>
    </tab>
    <tab title="json" group-key="config-json">
        <code-block lang="json">
            {
                "migration": {
                    "down-required": false
                }
            }
        </code-block>
    </tab>
    <tab title="env" group-key="config-env">
        <code-block lang="shell">
            export DINGO_MIGRATION__DOWN_REQUIRED=false
        </code-block>
    </tab>
</tabs>

> **Allowed values**: `true` | `false`
{style="info"}

## log.level

Specifies logging level of the application

<tabs group="config-type">
    <tab title="yaml" group-key="config-yaml">
        <code-block lang="yaml">
            log:
              level: Information
        </code-block>
    </tab>
    <tab title="json" group-key="config-json">
        <code-block lang="json">
            {
                "log": {
                    "level": "Information"
                }
            }
        </code-block>
    </tab>
    <tab title="env" group-key="config-env">
        <code-block lang="shell">
            export DINGO_LOG__LEVEL=Information
        </code-block>
    </tab>
</tabs>

> **Allowed values**: `Trace` | `Debug` | `Information` | `Warning` | `Error` | `Critical` | `None`
{style="info"}

## Example config

<tabs group="config-type">
    <tab title="yaml" group-key="config-yaml">
        <code-block lang="yaml">
            db:
              connection-string: Server=127.0.0.1;Database=dingo;User Id=dingo_usr;Password=VeryStr0ngPwd#;
              provider: SqlServer
              schema: dingo
            migration:
              delimiter: '^--\s*down$'
              wildcard: *.sql
              down-required: true
            log:
              level: Error
        </code-block>
    </tab>
    <tab title="json" group-key="config-json">
        <code-block lang="json">
            {
                "db": {
                    "connection-string": "Server=127.0.0.1;Database=dingo;User Id=dingo_usr;Password=VeryStr0ngPwd#;",
                    "provider": "SqlServer",
                    "schema": "dingo"
                },
                "migration": {
                    "delimiter": "^--\s*down$"
                    "wildcard": "*.sql"
                    "down-required": true
                },
                "log": {
                    "level": "Error"
                }
            }
        </code-block>
    </tab>
    <tab title="env" group-key="config-env">
        <code-block lang="shell">
            export DINGO_DB__CONNECTION_STRING="Server=127.0.0.1;Database=dingo;User Id=dingo_usr;Password=VeryStr0ngPwd#;"
            export DINGO_DB__PROVIDER=SqlServer
            export DINGO_DB__SCHEMA=dingo
            export DINGO_MIGRATION__DELIMITER="^--\s*down$"
            export DINGO_MIGRATION__WILDCARD="*.sql"
            export DINGO_MIGRATION__DOWN_REQUIRED=true
            export DINGO_LOG__LEVEL=Error
        </code-block>
    </tab>
</tabs>