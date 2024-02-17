using Microsoft.EntityFrameworkCore;
using SampleApi.Domain;
using SampleApi.Domain.Repositories;

namespace SampleApi.Database.Repositories;

internal class CustomerRepository : ICustomerRepository
{
    private SampleAppDbContext _dbContext;
    public CustomerRepository(SampleAppDbContext sampleAppDbContext)
    {
        _dbContext = sampleAppDbContext;
    }

    public List<Customer> GetCustomers()
    {
        var customers = _dbContext.Customers;

        return customers.ToList();
    }

    public void AddCustomer(Customer customer)
    {
        _dbContext.Customers.Add(customer);

        _dbContext.SaveChanges();
    }

    public Customer GetCustomer(long id)
    {
        var customer = _dbContext.Customers.FirstOrDefault(x => x.Id == id);

        return customer;
    }

    public List<Address> Addresses(long id)
    {
        var addressIds = _dbContext.CustomerAddresses
            .Where(o => o.CustomersId == id)
            .Select(o => o.AddressId)
            .ToList();

        var addresses = _dbContext.Addresses.Where(o => addressIds.Contains(id)).ToList();

        return addresses;
    }
}
