using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleApi.Database.Concurrency;
using SampleApi.Database.Repositories;
using SampleApi.Domain;

namespace SampleApi.Database;

internal class CouponCountService : ICouponCountService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CouponCountService> _logger;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public CouponCountService(IServiceProvider serviceProvider, ILogger<CouponCountService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void UpdateCouponCount(string code)
    {
        try
        {
            _lock.Wait();

            ConcurrencyUtilities.ExecuteAsync(async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                await using var dbContext = scope.ServiceProvider.GetRequiredService<SampleAppDbContext>();

                var coupon = dbContext.Coupons.Where(o => o.Code == code).FirstOrDefault();

                if (coupon != null)
                {
                    var repository = new CouponRepository(dbContext);

                    repository.UpdateCouponCount(coupon);

                    dbContext.SaveChangesAsyncWithConcurrencyCheckAsync();
                }

            }, _logger);
        }
        finally
        {
            _lock.Release();
        }
    }
}