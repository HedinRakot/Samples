using Microsoft.Extensions.DependencyInjection;

namespace SampleApi.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddScoped<ITestCommandHandler, TestCommandHandler>();
    }
}
