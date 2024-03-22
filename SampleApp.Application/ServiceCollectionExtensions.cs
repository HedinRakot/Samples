using Microsoft.Extensions.DependencyInjection;
using SampleApp.Tracing;

namespace SampleApp.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var httpClientBuilder = services.AddHttpClient<IOrderHttpClient, OrderHttpClient>(
            delegate (HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://localhost:5100/");
        });

        httpClientBuilder.Services.AddTransient<TracingDelegatingHandler>();
        httpClientBuilder.AddHttpMessageHandler<TracingDelegatingHandler>();


        return services.AddScoped<IOrderService, OrderService>()
        .AddSingleton<IMessageService, MessageService>()
        .AddSingleton<ICache, Cache>()
        .AddHostedService<SampleBackgroundService>();
    }
}
