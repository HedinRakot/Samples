using System.Diagnostics;

namespace SampleApp.Application;

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
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(DiagnosticListener value)
    {
        if (value.Name == "Microsoft.AspNetCore")
        {
            value.Subscribe(this);
        }
    }

    public void OnNext(KeyValuePair<string, object> value)
    {
        if (value.Key == "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start")
        {
            //if(value.Value is HttpContext)
            //{
            //    var httpContext = value.Value as HttpContext;
            //}

            if (value.Value is HttpContext httpContext)
            {
                var correlationId = httpContext.Request.Headers["x-correlation-id"].FirstOrDefault();

                if(correlationId != null)
                {
                    _tracingManager.BeginTracing(correlationId);
                }
                else
                {
                    _tracingManager.BeginTracing(Guid.NewGuid().ToString());
                }
            }
        }
        else if (value.Key == "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop")
        {
            _tracingManager.EndTracing();
        }
    }
}

