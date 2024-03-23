using Microsoft.Extensions.DependencyInjection;

namespace SampleApp.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddSingleton<IMessageService, MessageService>()
            .AddSingleton<ICache, Cache>()
            .AddSingleton<ITracingManager, TracingManager>()
            .AddScoped<IOrderService, OrderService>()
            .AddHostedService<SampleBackgroundService>();
    }
}
