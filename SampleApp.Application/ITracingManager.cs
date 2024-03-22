namespace SampleApp.Tracing;

public interface ITracingManager
{
    string? CorrelationId { get; set; }

    void BeginTracing(string? correlationId);

    void EndTracing();
}