namespace SampleApi.Database.Concurrency;

public abstract class ConcurrencyException : Exception
{
    protected ConcurrencyException()
    {
    }

    protected ConcurrencyException(string message)
        : base(message)
    {
    }

    protected ConcurrencyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}