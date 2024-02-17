using SampleApi.Domain;

namespace SampleApi.Application;

public class CustomerRepository
{
    public CustomerRepository()
    {
        Customers.Add(new Customer
        {
            Id = 1,
            Name = "TestCustom",
            LastName = "Lastname",
            Email = "email@test.de",
            CustomValidationField = "Yury",
            Number = 1
        });
    }
    public List<Customer> Customers { get; set; } = new List<Customer>();
}