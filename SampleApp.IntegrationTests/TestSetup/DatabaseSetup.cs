using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampeApp.Database.Migrations.Migrations;
using FluentMigrator.Runner;

namespace SampleApp.IntegrationTests.TestSetup;

public class DatabaseSetup : IDisposable
{
    private readonly string _sampeAppDbConnectionString;

    public DatabaseSetup(string configFile)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(configFile, true, true)
            .Build();

        _sampeAppDbConnectionString = configuration.GetConnectionString("SampleAppDb");

        var connectionStringBuilder = new SqlConnectionStringBuilder(_sampeAppDbConnectionString);
        var initialCatalog = connectionStringBuilder.InitialCatalog;
        connectionStringBuilder.Remove("Initial Catalog");

        using (var sqlConnection = new SqlConnection(connectionStringBuilder.ConnectionString)) 
        {
            sqlConnection.Open();

            var command = $"SELECT * FROM sys.databases WHERE NAME ='{initialCatalog}'";
            
            using var sqlCommand = new SqlCommand(command, sqlConnection);
            using var sqlDataReader = sqlCommand.ExecuteReader();

            if (sqlDataReader.HasRows)
            {
                var dropCommand = $"USE master; ALTER DATABASE {initialCatalog} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE {initialCatalog}";
                using var dropSqlCommand = new SqlCommand(dropCommand, sqlConnection);
                dropSqlCommand.ExecuteNonQuery();
            }
        }

        var dbContextOptions = new DbContextOptionsBuilder<TestDatabaseDbContext>()
           .UseSqlServer(_sampeAppDbConnectionString)
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
           .Options;

        using var dbContext = new TestDatabaseDbContext(dbContextOptions);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();


        new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddSqlServer()
                .WithGlobalConnectionString(_sampeAppDbConnectionString)
                .ScanIn(typeof(Initial).Assembly).For.Migrations()
            )
            .BuildServiceProvider(false)
            .GetRequiredService<IMigrationRunner>()
            .MigrateUp();
    }

    public void Dispose()
    {
        //TODO DB Delete
    }
}
