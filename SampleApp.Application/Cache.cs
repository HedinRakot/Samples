using Microsoft.Extensions.DependencyInjection;
using SampleApp.Domain.Repositories;

namespace SampleApp.Application;

public class Cache : ICache
{
    private IReadOnlyCollection<Domain.Customer> _customers;
    private IServiceProvider _serviceProvider;

    private readonly object _lockObject = new();
    private bool _isInitialized;
    private Task? _initTask;

    public Cache(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IReadOnlyCollection<Domain.Customer>> GetCustomers()
    {
        await WaitUntilInitialized();

        return _customers;
    }

    public Task Refresh(CancellationToken cancellationToken)
    {
        lock(_lockObject)
        {
            return _initTask ?? RefreshInternal(!_isInitialized, cancellationToken);
        }
    }


    internal async Task RefreshData(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

        _customers = customerRepository.GetCustomers();
    }


    internal Task WaitUntilInitialized()
    {
        lock (_lockObject)
        {
            if (_isInitialized)
            {
                return Task.CompletedTask;
            }

            return _initTask != null
                   ? _initTask
                   : RefreshInternal(true, CancellationToken.None);
        }
    }

    internal async Task RefreshInternal(bool isInitializing, CancellationToken cancellationToken)
    {
        try
        {
            if (isInitializing)
            {
                _initTask = RefreshData(cancellationToken);

                await _initTask;
            }
            else
            {
                await RefreshData(cancellationToken);
            }

            _isInitialized = true;
        }
        finally
        {
            _initTask = null;
        }
    }
}

