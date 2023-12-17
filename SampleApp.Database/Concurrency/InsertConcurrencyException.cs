namespace SampleApp.Database.Concurrency;

public class InsertConcurrencyException : ConcurrencyException
{
    public InsertConcurrencyException(string message, Exception innerException)
        : base(message ?? "Entity exists jet", innerException)
    {
    }

    public InsertConcurrencyException(Exception innerException)
        : this(null, innerException)
    {
    }
}
