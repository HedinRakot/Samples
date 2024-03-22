namespace SampleApp.Tracing;

public class TracingManager : ITracingManager
{
    public string? CorrelationId { get; set; }

    public TracingManager()
    {
    }

    public void BeginTracing(string? correlationId)
    {
        if (correlationId != null)
        {
            CorrelationId = correlationId;
        }
        else
        {
            CorrelationId = Guid.NewGuid().ToString();
        }
    }

    public void EndTracing()
    {
        CorrelationId = null;
    }
}

