namespace SampleApp.Database.Concurrency;

public class UpdateConcurrentException : ConcurrencyException
{
    public UpdateConcurrentException(string message, Exception innerException)
        : base(
            message ??
            "Die Entität wurde während der Bearbeitung durch einen anderen Prozess geändert oder gelöscht.",
            innerException)
    {
    }

    public UpdateConcurrentException(Exception innerException)
        : this(null, innerException)
    {
    }
}
