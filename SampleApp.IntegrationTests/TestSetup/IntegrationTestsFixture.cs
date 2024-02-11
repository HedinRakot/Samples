using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using SampleApp.Application;
using SampleApp.Database;
using System.Reflection;

namespace SampleApp.IntegrationTests.TestSetup;

public class IntegrationTestsFixture : WebApplicationFactory<Program>
{
    private readonly DatabaseSetup _dbSetup;
    public IOrderService TestOrderService = Substitute.For<IOrderService>();

    public IntegrationTestsFixture()
    {
        _dbSetup = new DatabaseSetup("appsettings.integrationtests.json");
    }

    protected override void Dispose(bool disposing)
    {
        try
        {
            base.Dispose(disposing);

            if(disposing)
            {
                _dbSetup.Dispose();
            }
        }
        catch (Exception ex)
        {
            //logging or something else 
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests")
            .ConfigureAppConfiguration(config =>
            {
                //config.AddJsonFile("appsettings.integrationtests.json");

                config.AddJsonFile(Path.Combine(
                    Path.GetDirectoryName(Assembly.GetAssembly(typeof(IntegrationTestsFixture)).Location),
                    "appsettings.integrationtests.json"
                ));
            })
            .ConfigureTestServices(services =>
            {
                //TODO mock api
                services.AddSingleton<IOrderService>(TestOrderService);
            });

        base.ConfigureWebHost(builder);
    }

    public Domain.Order? GetOrder(string orderNumber)
    {
        using var scope = Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<SampleAppDbContext>();

        return dbContext.Orders.FirstOrDefault(o => o.OrderNumber == orderNumber);
    }
}