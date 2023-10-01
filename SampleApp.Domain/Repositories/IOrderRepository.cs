namespace SampleApp.Domain.Repositories;

public interface IOrderRepository
{
    List<Order> GetOrders();
}