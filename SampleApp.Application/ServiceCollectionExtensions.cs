using Microsoft.Extensions.DependencyInjection;

namespace SampleApp.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddScoped<IOrderService, OrderService>()
        .AddSingleton<IMessageService, MessageService>();
    }
}
