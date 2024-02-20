﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleApi.Database.Repositories;
using SampleApi.Domain;
using SampleApi.Domain.Repositories;

namespace SampleApi.Database;

public static class DbServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var result = services.AddDbContext<SampleAppDbContext>(options =>
        {
            //options.UseLazyLoadingProxies();
            options.UseSqlServer(configuration.GetConnectionString("SampleAppDb"));
        });

        return result.AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<IOrderHistoryRepository, OrderHistoryRepository>()
            .AddScoped<ICouponRepository, CouponRepository>()
            .AddScoped<ICouponCountService, CouponCountService>()
            .AddScoped<ISqlUnitOfWork, SqlUnitOfWork>();
    }
}