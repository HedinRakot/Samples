using Microsoft.EntityFrameworkCore;
using SampleApp.Domain;
using SampleApp.Domain.Repositories;

namespace SampleApp.Database.Repositories;

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
            coupon.AppliedCount = 0;
        else
            coupon.AppliedCount++;

        _dbContext.Update(coupon);

        return coupon;
    }
}
