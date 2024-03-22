using System;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApp.Tracing;

public class TracingDelegatingHandler : DelegatingHandler
{
	private readonly IServiceProvider _serviceProvider;

	public TracingDelegatingHandler(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        PrepareRequest(request);

        return base.Send(request, cancellationToken);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        PrepareRequest(request);

        return base.SendAsync(request, cancellationToken);
    }

    private void PrepareRequest(HttpRequestMessage requestMessage)
    {
        var tracingManager = _serviceProvider.GetRequiredService<ITracingManager>();

        if (!string.IsNullOrEmpty(tracingManager.CorrelationId) &&
            !requestMessage.Headers.Contains("x-correlation-id"))
        {
            requestMessage.Headers.Add("x-correlation-id", tracingManager.CorrelationId);
        }
    }
}

