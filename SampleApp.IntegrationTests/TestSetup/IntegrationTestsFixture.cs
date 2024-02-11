using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace SampleApp.IntegrationTests.TestSetup;

public class IntegrationTestsFixture : WebApplicationFactory<Program>
{
    private readonly DatabaseSetup _dbSetup;
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
            });

        base.ConfigureWebHost(builder);
    }
}