using SampleApp.Domain;

namespace SampleApp.Application;

public class OrderRepository
{
    public List<Order> Orders { get; set; } = new List<Order>();
}