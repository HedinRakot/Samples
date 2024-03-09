using SampleApp.Domain;

namespace SampleApp.Application;

public interface ICache
{
    Task<IReadOnlyCollection<Customer>> GetCustomers();

    Task Refresh(CancellationToken cancellationToken);
}