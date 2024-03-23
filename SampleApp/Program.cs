using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using NServiceBus;
using SampleApp;
using SampleApp.Application;
using SampleApp.Authentication;
using SampleApp.Database;
using SampleApp.Domain.Settings;
using SampleApp.ErrorHandling;
using SampleApp.Messages;
using SampleApp.Models;
using SampleApp.Models.Mapping;
using Serilog;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    //ContentRootPath = AppContext.BaseDirectory, //notwendig für Hosting als Windows Service
    Args = args
});


try
{
    var loggerConfiguration = new LoggerConfiguration()
    //.MinimumLevel.Override("Microsoft", LogEventLevel.)
    .Enrich.FromLogContext()
    .WriteTo.File("SampleApp_Fatal.log");
    Log.Logger = loggerConfiguration.CreateBootstrapLogger();


    // Add services to the container.
    builder.Services
        .AddControllersWithViews()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        }
    );

    builder.Services.AddSingleton<IMapping<SampleApp.Domain.Customer, CustomerModel>, CustomerModelMappingInterface>();

    builder.Services
        .AddApplication()
        .AddDatabase(builder.Configuration);


    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Unspecified;
            options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
            options.LoginPath = "/Login/SignIn/";
            options.AccessDeniedPath = "/Error/Forbidden/";
        })
        .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationScheme.DefaultScheme, null);

    builder.Services.AddAuthorization(o =>
    {
        o.AddPolicy(AuthorizeControllerModelConvention.PolicyName, policy =>
        {
            policy.RequireAuthenticatedUser();
        });
    });

    builder.Services.AddMvc(options =>
    {
        options.Conventions.Add(new AuthorizeControllerModelConvention());
    });


    builder.Services.Configure<ApiKeyAuthenticationOptions>(
        builder.Configuration.GetSection(ApiKeyAuthenticationOptions.SectionName));




    //NServiceBus
    var endpointConfiguration = new EndpointConfiguration("SampleApp");
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

    var endpointContainer = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, builder.Services);
    var endpointInstance = await endpointContainer.Start(builder.Services.BuildServiceProvider());


    builder.Services.AddSingleton<NServiceBus.IMessageSession>(endpointInstance);

    //logging
    loggerConfiguration = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProcessId()
        .Enrich.WithProcessName()
        .Enrich.WithMachineName();

    var logger = loggerConfiguration.CreateLogger();
    builder.Host.UseSerilog(logger);

    //tracing
    builder.Services.AddSingleton<TracingListener>();

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

    app.UseSerilogRequestLogging();


    var tracingListener = app.Services.GetRequiredService<TracingListener>();
    DiagnosticListener.AllListeners.Subscribe(tracingListener);

    await app.RunAsync();


    await endpointInstance.Stop()
        .ConfigureAwait(false);
}
catch(Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.CloseAndFlush();
}



namespace SampleApp
{
    public class Program
    {

    }
}