using SampleApp.Domain;

namespace SampleApp.Application;

public interface IOrderHttpClient
{
    Task<List<Order>> GetOrders();
}