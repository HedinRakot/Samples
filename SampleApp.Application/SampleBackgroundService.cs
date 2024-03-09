using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SampleApp.Application;

public class SampleBackgroundService : BackgroundService
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SampleBackgroundService> _logger;

    private readonly TimeSpan _refreshInterval = TimeSpan.FromSeconds(10);

	public SampleBackgroundService(IHostApplicationLifetime applicationLifetime,
        IServiceProvider serviceProvider,
        ILogger<SampleBackgroundService> logger)
	{
        _applicationLifetime = applicationLifetime;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken,
                _applicationLifetime.ApplicationStarted);

            await Task.Delay(-1, tokenSource.Token);
        }
        catch(TaskCanceledException)
        {
            //wait until App started
        }


        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var cache = scope.ServiceProvider.GetRequiredService<ICache>();

                await cache.Refresh(stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during .....");
            }

            try
            {
                await Task.Delay(_refreshInterval, stoppingToken);
            }
            catch(TaskCanceledException)
            {
                break;
            }
        }
    }
}

