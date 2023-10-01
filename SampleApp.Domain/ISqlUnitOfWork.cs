using SampleApp.Domain.Repositories;

namespace SampleApp.Database;

public interface ISqlUnitOfWork
{
    IOrderHistoryRepository OrderHistoryRepository { get; }
    IOrderRepository OrderRepository { get; }

    void SaveChanges();
}