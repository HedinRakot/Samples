using SampleApp.Database.Repositories;
using SampleApp.Domain.Repositories;

namespace SampleApp.Database;

internal class SqlUnitOfWork : ISqlUnitOfWork
{
    private SampleAppDbContext _dbContext;
    private IOrderRepository _orderRepository;
    private IOrderHistoryRepository _orderHistoryRepository;
    private ICustomerRepository _customerRepository;

    public SqlUnitOfWork(SampleAppDbContext dbContext)
    {
        _dbContext = dbContext;
        _orderRepository = new OrderRepository(dbContext);
        _orderHistoryRepository = new OrderHistoryRepository(dbContext);
        _customerRepository = new CustomerRepository(dbContext);
    }

    public IOrderRepository OrderRepository => _orderRepository;
    public IOrderHistoryRepository OrderHistoryRepository => _orderHistoryRepository;
    public ICustomerRepository CustomerRepository => _customerRepository;

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}