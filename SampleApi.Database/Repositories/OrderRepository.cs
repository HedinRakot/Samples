using Microsoft.EntityFrameworkCore;
using SampleApi.Domain;
using SampleApi.Domain.Repositories;

namespace SampleApi.Database.Repositories;

internal class OrderRepository : IOrderRepository
{
    private SampleAppDbContext _dbContext;
    public OrderRepository(SampleAppDbContext sampleAppDbContext)
    {
        _dbContext = sampleAppDbContext;
    }

    public List<Order> GetOrderWithCustomers()
    {
        var orders = _dbContext.Orders.Include(o => o.Customer);

        return orders.ToList();
    }

    public List<Order> GetOrders()
    {
        var orders = _dbContext.Orders;

        return orders.ToList();
    }

    public Order AddOrder(Order order)
    {
        _dbContext.Orders.Add(order);

        return order;
    }

    public Order GetById(long id)
    {
        return _dbContext.Orders
            .Include(o => o.Customer)
            .FirstOrDefault(o => o.Id == id);
    }

    public Order GetByIdWithHistory(long id)
    {
        return _dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.History)
            .FirstOrDefault(o => o.Id == id);
    }

    public Order UpdateOrder(Order order)
    {
        _dbContext.SaveChanges();

        return order;
    }
}
