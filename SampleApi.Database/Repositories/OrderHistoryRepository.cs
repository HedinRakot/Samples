using SampleApi.Domain;
using SampleApi.Domain.Repositories;

namespace SampleApi.Database.Repositories;

internal class OrderHistoryRepository : IOrderHistoryRepository
{
    private SampleAppDbContext _dbContext;
    public OrderHistoryRepository(SampleAppDbContext sampleAppDbContext)
    {
        _dbContext = sampleAppDbContext;
    }

    public OrderHistory AddOrderHistory(Order order)
    {
        var newHistoryItem = new OrderHistory
        {
            ChangeDate = DateTimeOffset.Now,
            Changes = "Order new created"
        };

        order.History.Add(newHistoryItem);

        return newHistoryItem;
    }
}
