using SampleApp.Database.Repositories;
using SampleApp.Domain.Repositories;

namespace SampleApp.Database;

internal class SqlUnitOfWork : ISqlUnitOfWork
{
    private SampleAppDbContext _dbContext;
    private IOrderRepository _orderRepository;
    private IOrderHistoryRepository _orderHistoryRepository;

    public SqlUnitOfWork(SampleAppDbContext dbContext)
    {
        _dbContext = dbContext;
        _orderRepository = new OrderRepository(dbContext);
        _orderHistoryRepository = new OrderHistoryRepository(dbContext);
    }

    public IOrderRepository OrderRepository => _orderRepository;
    public IOrderHistoryRepository OrderHistoryRepository => _orderHistoryRepository;

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}