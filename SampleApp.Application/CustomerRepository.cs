using SampleApp.Domain;

namespace SampleApp.Application;

public class CustomerRepository
{
    public CustomerRepository()
    {
        Customers.Add(new Customer
        {
            Id = 1,
            Name = "Test Customer",
        });
    }
    public List<Customer> Customers { get; set; } = new List<Customer>();
}