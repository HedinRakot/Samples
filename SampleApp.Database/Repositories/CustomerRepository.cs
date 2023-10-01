﻿using SampleApp.Domain;
using SampleApp.Domain.Repositories;

namespace SampleApp.Database.Repositories;

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
}