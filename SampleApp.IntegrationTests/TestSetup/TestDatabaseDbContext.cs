using Microsoft.EntityFrameworkCore;

namespace SampleApp.IntegrationTests.TestSetup;

internal sealed class TestDatabaseDbContext : DbContext
{
    public TestDatabaseDbContext(DbContextOptions<TestDatabaseDbContext> options) : base(options)
    {
        
    }
}
