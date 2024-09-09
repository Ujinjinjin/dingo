// See https://aka.ms/new-console-template for more information

using Dingo.Core.Extensions;
using Dingo.Core.IO;
using Dingo.Core.Services.Handlers;
using Dingo.Playground;
using Microsoft.Extensions.DependencyInjection;

var sc = new ServiceCollection();

sc.AddDingo();
sc.AddSingleton<IOutput, ConsoleOutput>();

var sp = sc.BuildServiceProvider();
sp.UseDingo();

var handler = sp.GetService<IMigrationHandler>();
// await handler.ShowStatusAsync("sqlserver", "/Users/camillegalladjov/DataGripProjects/dingo/sql-serve-migrations");
// await handler.MigrateAsync("sqlserver", "/Users/camillegalladjov/DataGripProjects/dingo/sql-serve-migrations");
await handler.RollbackAsync("sqlserver", "/Users/camillegalladjov/DataGripProjects/dingo/sql-serve-migrations", 1, false);

// await handler.MigrateAsync(default, "/Users/camillegalladjov/DataGripProjects/dingo/migrations");
// await handler.RollbackAsync("/Users/camillegalladjov/DataGripProjects/dingo/migrations", 1, false);

// var handler = sp.GetService<IConnectionHandler>();
// await handler.HandshakeAsync("sqlserver");

// var handler = sp.GetService<IConfigHandler>();
// handler.Init(default, "sqlserver");

// var loggerFactory = sp.GetService<ILoggerFactory>();
// var logger = loggerFactory.CreateLogger<Program>();
// logger.LogInformation("f");
// logger.LogError(new Exception("Some exception message"), "f");
