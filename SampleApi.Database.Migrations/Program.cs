using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

//Console.WriteLine("Start DB migration");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", false)
    .Build();

var databaseName = "SampleAppDb";

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


var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
connectionStringBuilder.Remove("initial catalog");
connectionStringBuilder.Remove("database");
using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
var checkDbExists = new SqlCommand($"SELECT database_id FROM sys.databases WHERE Name = '{databaseName}'",
    connection);
connection.Open();
var result = checkDbExists.ExecuteScalar();
if (result is null)
{
    using var command = new SqlCommand($"CREATE DATABASE {databaseName}", connection);
    command.ExecuteNonQuery();
    Console.WriteLine($"Database {databaseName} created successfully!");
}
connection.Close();
connection.Dispose();

var runner = serviceCollection.GetRequiredService<IMigrationRunner>();

runner.MigrateUp();