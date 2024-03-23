namespace SampleApp.Application;

public interface ITracingManager
{
    string? CorrelationId { get; set; }

    void BeginTracing(string? correlationId);
    void EndTracing();
}