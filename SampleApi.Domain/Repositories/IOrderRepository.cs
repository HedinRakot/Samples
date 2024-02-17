namespace SampleApi.Domain.Repositories;

public interface IOrderRepository
{
    List<Order> GetOrders();
    List<Order> GetOrderWithCustomers();
    Order AddOrder(Order order);
    Order GetById(long id);
    Order GetByIdWithHistory(long id);

    Order UpdateOrder(Order order);
}