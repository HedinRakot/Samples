using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Console;
using NServiceBus;
using SampleApi;
using SampleApi.Application;
using SampleApi.Authentication;
using SampleApi.Database;
using SampleApi.ErrorHandling;
using SampleApi.Models;
using SampleApi.Models.Mapping;
using SampleApp.Messages;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    //ContentRootPath = AppContext.BaseDirectory, //notwendig für Hosting als Windows Service
    Args = args
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<CustomerRepository>();
builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddSingleton<IMapping<SampleApi.Domain.Customer, CustomerModel>, CustomerModelMappingInterface>();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddAuthentication(ApiKeyAuthenticationScheme.DefaultScheme)
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationScheme.DefaultScheme, null);

//builder.Services.AddScoped<IAuthorizationHandler, ApiKeyAuthorizationHandler>();
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("SampleApiPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes(ApiKeyAuthenticationScheme.DefaultScheme);
        policy.AddRequirements(new ApiKeyRequirement());
    });

    o.AddPolicy(AuthorizeControllerModelConvention.PolicyName, policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddMvc(options =>
{
    options.Conventions.Add(new AuthorizeControllerModelConvention());
});


builder.Logging.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Enabled);




//NServiceBus
var endpointConfiguration = new EndpointConfiguration("SampleApi");
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.EnableInstallers();

// Choose JSON to serialize and deserialize messages
endpointConfiguration.UseSerialization<NServiceBus.SystemJsonSerializer>();

var nserviceBusConnectionString = builder.Configuration.GetConnectionString("NServiceBus");

var transportConfig = new NServiceBus.SqlServerTransport(nserviceBusConnectionString)
{
    DefaultSchema = "dbo",
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive,
    Subscriptions =
    {
        CacheInvalidationPeriod = TimeSpan.FromMinutes(1),
        SubscriptionTableName = new NServiceBus.Transport.SqlServer.SubscriptionTableName(
            table: "Subscriptions",
            schema: "dbo")
    }
};

transportConfig.SchemaAndCatalog.UseSchemaForQueue("error", "dbo");
transportConfig.SchemaAndCatalog.UseSchemaForQueue("audit", "dbo");

var transport = endpointConfiguration.UseTransport<SqlServerTransport>(transportConfig);
transport.RouteToEndpoint(typeof(TestCommand), "SampleApi");

//persistence
var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
dialect.Schema("dbo");
persistence.ConnectionBuilder(() => new SqlConnection(nserviceBusConnectionString));
persistence.TablePrefix("");

await SqlServerHelper.CreateSchema(nserviceBusConnectionString, "dbo");

var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration);


//builder.Host.UseWindowsService(); //notwendig für Hosting als Windows Service

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=Index}/{id?}");

app.UseMiddleware<ErrorHandlingMiddleware>();


await app.RunAsync();

await endpointInstance.Stop()
    .ConfigureAwait(false);