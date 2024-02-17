using Microsoft.EntityFrameworkCore;

namespace SampleApi.Database.Concurrency;

public static class DbContextExtensions
{
    public static void SaveChangesAsyncWithConcurrencyCheckAsync(this DbContext dbContext)
    {
        DbAccessUtilities.ExecuteWithConcurrencyCheckAsync(() => dbContext.SaveChanges());
    }

    //public static Task SaveChangesAsyncWithConcurrencyCheckAsync(this DbContext dbContext,
    //    CancellationToken cancellationToken) =>
    //    DbAccessUtilities.ExecuteWithConcurrencyCheckAsync(async () =>
    //    {
    //        await dbContext.SaveChangesAsync(cancellationToken);
    //    });
}