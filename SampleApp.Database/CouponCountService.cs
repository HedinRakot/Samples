using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleApp.Database.Concurrency;
using SampleApp.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Database;

internal class CouponCountService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CouponCountService> _logger;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private int? _instanzId;

    public CouponCountService(IServiceProvider serviceProvider, ILogger<CouponCountService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async ValueTask<int> GetInstanzId(CancellationToken cancellationToken)
    {
        if (_instanzId != null)
        {
            return _instanzId.Value;
        }

        try
        {
            await _lock.WaitAsync(cancellationToken);
            _instanzId = await CreateNewInstanzId(cancellationToken);
        }
        finally
        {
            _lock.Release();
        }

        return _instanzId.Value;
    }

    private Task<int> CreateNewInstanzId(CancellationToken cancellationToken) =>
        ConcurrencyUtilities.ExecuteAsync(async () =>
        {
            using var scope = _serviceProvider.CreateScope();
            await using var dbContext = scope.ServiceProvider.GetRequiredService<SampleAppDbContext>();

            var repository = new CouponRepository(dbContext);

            var instanzId = await repository.UpdateCouponCount(cancellationToken);

            await dbContext.SaveChangesAsyncWithConcurrencyCheckAsync(cancellationToken);

            return instanzId;
        }, _logger);
}