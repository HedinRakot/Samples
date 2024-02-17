using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SampleApi.Database.Concurrency;

public static class DbAccessUtilities
{
    public static void ExecuteWithConcurrencyCheckAsync(Func<int> action)
    {
        try
        {
            action();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new UpdateConcurrentException(e);
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException sqlException &&
                                          sqlException.IsPrimaryKeyViolationError())
        {
            throw new InsertConcurrencyException(e);
        }
    }


    //public static async Task ExecuteWithConcurrencyCheckAsync(Func<Task> action)
    //{
    //    try
    //    {
    //        await action();
    //    }
    //    catch (DbUpdateConcurrencyException e)
    //    {
    //        throw new UpdateConcurrentException(e);

    //    }
    //    catch (DbUpdateException e) when (e.InnerException is SqlException sqlException &&
    //                                      sqlException.IsPrimaryKeyViolationError())
    //    {
    //        throw new InsertConcurrencyException(e);
    //    }
    //}
}