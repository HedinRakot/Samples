namespace SampleApi.Domain.Repositories;

public interface IOrderHistoryRepository
{
    OrderHistory AddOrderHistory(Order order);
}