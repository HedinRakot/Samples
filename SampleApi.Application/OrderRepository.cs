using SampleApi.Domain;

namespace SampleApi.Application;

public class OrderRepository
{
    public List<Order> Orders { get; set; } = new List<Order>();
}