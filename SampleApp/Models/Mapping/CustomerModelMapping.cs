namespace SampleApp.Models.Mapping;

public static class CustomerModelMapping
{
    public static CustomerModel Map(Domain.Customer customer)
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

    public static Domain.Customer Map(CustomerModel customerModel)
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