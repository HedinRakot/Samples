using SampleApi.Domain.Repositories;

namespace SampleApi.Database;

public interface ISqlUnitOfWork
{
    IOrderHistoryRepository OrderHistoryRepository { get; }
    IOrderRepository OrderRepository { get; }
    ICustomerRepository CustomerRepository { get; }

    void SaveChanges();
}