using SampleApp.Domain;

namespace SampleApp.Application;

public interface IOrderService
{
    Task<List<Order>> GetOrders();

    Task<Order> AddOrder(Order order);
}