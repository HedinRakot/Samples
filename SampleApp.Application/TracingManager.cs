namespace SampleApp.Application;

public class TracingManager : ITracingManager
{
    public string? CorrelationId { get; set; }

    public TracingManager()
    {
    }

    public void BeginTracing(string? correlationId)
    {
        if (CorrelationId != null)
        {
            CorrelationId = correlationId;
        }
    }

    public void EndTracing()
    {
        CorrelationId = null;
    }
}

