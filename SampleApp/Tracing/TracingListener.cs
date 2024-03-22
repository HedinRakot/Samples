using System.Diagnostics;

namespace SampleApp.Tracing;

public class TracingListener : IObserver<DiagnosticListener>,
    IObserver<KeyValuePair<string, object>>
{
	private readonly ITracingManager _tracingManager;
    public TracingListener(ITracingManager tracingManager)
	{
		_tracingManager = tracingManager;
	}

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public void OnNext(DiagnosticListener value)
    {
        if(value.Name == "Microsoft.AspNetCore")
        {
            value.Subscribe(this);
        }
    }

    public void OnNext(KeyValuePair<string, object> value)
    {
        if(value.Key == "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start")
        {
            if(value.Value is HttpContext httpContext)
            {
                var correlationId = httpContext.Request.Headers["x-correlation-id"].FirstOrDefault();

                _tracingManager.BeginTracing(correlationId);
            }
        }
        else if (value.Key == "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop")
        {
            _tracingManager.EndTracing();
        }
    }
}

