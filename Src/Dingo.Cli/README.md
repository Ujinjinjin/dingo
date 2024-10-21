# Contributing

## Test coverage

We use `coverlet` to collect coverage information, which will be installed during nuget restore. But coverlet generates a separate `xml` report for each test project. To merge them into a single `html` report we use `reportgenerator`.

In order to install `reportgenerator` locally, run:

```shell
dotnet tool install dotnet-reportgenerator-globaltool
```
