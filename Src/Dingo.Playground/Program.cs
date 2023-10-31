// See https://aka.ms/new-console-template for more information

using Dingo.Core.Extensions;
using Dingo.Core.IO;
using Dingo.Core.Services.Config;
using Dingo.Core.Services.Handlers;
using Dingo.Playground;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var sc = new ServiceCollection();

sc.AddDingo();
sc.AddSingleton<IOutput, ConsoleOutput>();

var sp = sc.BuildServiceProvider();
sp.UseDingo();

var handler = sp.GetService<IMigrationHandler>();
await handler.ShowStatusAsync("/Users/camillegalladjov/DataGripProjects/dingo/migrations");
// await handler.MigrateAsync("/Users/camillegalladjov/DataGripProjects/dingo/migrations");
// await handler.RollbackAsync("/Users/camillegalladjov/DataGripProjects/dingo/migrations", 1, false);

// var handler = sp.GetService<IConnectionHandler>();
// await handler.HandshakeAsync();

// var handler = sp.GetService<IConfigGenerator>();
// handler.Generate(".");

// var loggerFactory = sp.GetService<ILoggerFactory>();
// var logger = loggerFactory.CreateLogger<Program>();
// logger.LogInformation("f");
// logger.LogError(new Exception("Some exception message"), "f");
