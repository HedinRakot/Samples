using Microsoft.EntityFrameworkCore;
using SampleApi.Domain;
using SampleApi.Domain.Repositories;

namespace SampleApi.Database.Repositories;

internal class CouponRepository : ICouponRepository
{
    private SampleAppDbContext _dbContext;
    public CouponRepository(SampleAppDbContext sampleAppDbContext)
    {
        _dbContext = sampleAppDbContext;
    }

    public Coupon UpdateCouponCount(Coupon coupon)
    {
        if (coupon.AppliedCount == null)
            coupon.AppliedCount = 1;
        else
            coupon.AppliedCount++;

        _dbContext.Update(coupon);

        return coupon;
    }
}
