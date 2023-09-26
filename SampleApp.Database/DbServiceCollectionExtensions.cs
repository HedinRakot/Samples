using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Database.Repositories;
using SampleApp.Domain.Repositories;

namespace SampleApp.Database;

public static class DbServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var result = services.AddDbContext<SampleAppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SampleAppDb"));
        });

        return result.AddScoped<ICustomerRepository, CustomerRepository>();
    }
}
