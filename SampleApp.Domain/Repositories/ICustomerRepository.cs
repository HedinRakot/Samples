namespace SampleApp.Domain.Repositories;

public interface ICustomerRepository
{
    List<Customer> GetCustomers();
    void AddCustomer(Customer customer);

    Customer GetCustomer(long id);

    List<Address> Addresses(long id);
}