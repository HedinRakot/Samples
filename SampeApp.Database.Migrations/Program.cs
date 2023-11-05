using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Start DB migration");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();


var connectionString = configuration.GetConnectionString("SampleAppDb");

var serviceCollection = new ServiceCollection()
    .AddFluentMigratorCore()
    .ConfigureRunner(runner => runner
        .AddSqlServer()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(Program).Assembly).For.Migrations()
    )
    .AddLogging(log => log.AddFluentMigratorConsole())
    .BuildServiceProvider();

var runner = serviceCollection.GetRequiredService<IMigrationRunner>();

runner.MigrateUp();