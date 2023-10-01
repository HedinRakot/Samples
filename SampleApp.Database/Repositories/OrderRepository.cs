using SampleApp.Domain;
using SampleApp.Domain.Repositories;

namespace SampleApp.Database.Repositories;

internal class OrderRepository : IOrderRepository
{
    private SampleAppDbContext _dbContext;
    public OrderRepository(SampleAppDbContext sampleAppDbContext)
    {
        _dbContext = sampleAppDbContext;
    }

    public List<Order> GetOrders()
    {
        var orders = _dbContext.Orders;

        return orders.ToList();
    }
}
