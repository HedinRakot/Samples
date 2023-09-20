using SampleApp.Domain;

namespace SampleApp.Models.Mapping;

public interface ICustomerModelMappingInterface
{
    CustomerModel Map(Customer customer);
    Customer Map(CustomerModel customerModel);
}

public interface IMapping<TIn, TOut>
{
    TOut Map(TIn model);
    TIn Map(TOut domain);
}

public class CustomerModelMappingInterface : IMapping<Domain.Customer, CustomerModel>
{
    public CustomerModel Map(Domain.Customer customer)
    {
        return new CustomerModel
        {
            Id = customer.Id,
            Name = customer.Name,
            LastName = customer.LastName,
            Number = customer.Number,
            Email = customer.Email,
            CustomValidationField = customer.CustomValidationField
        };
    }

    public Domain.Customer Map(CustomerModel customerModel)
    {
        return new Domain.Customer
        {
            Id = customerModel.Id,
            Name = customerModel.Name,
            LastName = customerModel.LastName,
            Number = customerModel.Number,
            Email = customerModel.Email,
            CustomValidationField = customerModel.CustomValidationField
        };
    }
}