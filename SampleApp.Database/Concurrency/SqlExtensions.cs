using Microsoft.Data.SqlClient;

namespace SampleApp.Database.Concurrency;

public static class SqlExtensions
{
    public static bool IsPrimaryKeyViolationError(this SqlException exception) =>
        exception?.Errors.Cast<SqlError>().Any(e => e.Class == 14 && (e.Number == 2627 || e.Number == 2601)) == true;
}