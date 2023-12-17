using Microsoft.Extensions.Logging;

namespace SampleApp.Database.Concurrency;

public static class ConcurrencyUtilities
{
    public static async Task<T> ExecuteAsync<T>(Func<Task<T>> action, ILogger logger)
    {
        var attemptsCount = 0;
        while (true)
        {
            try
            {
                var result = await action();
                return result;
            }
            catch (ConcurrencyException e)
            {
                logger.LogWarning(e,
                    $"{nameof(ConcurrencyException)} beim {attemptsCount + 1}. Versuch aufgetreten.");
                attemptsCount++;
                if (attemptsCount >= 3)
                    throw;
            }
        }
    }

    public static Task ExecuteAsync(Func<Task> action, ILogger logger) =>
        ExecuteAsync<object>(async () =>
        {
            await action();
            return null;
        }, logger);
}
