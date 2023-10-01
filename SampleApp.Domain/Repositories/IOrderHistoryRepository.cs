namespace SampleApp.Domain.Repositories;

public interface IOrderHistoryRepository
{
    OrderHistory AddOrderHistory(Order order);
}