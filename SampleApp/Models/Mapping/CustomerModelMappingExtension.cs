namespace SampleApp.Models.Mapping;

public static class CustomerModelMappingExtension
{
    public static CustomerModel ToModel(this Domain.Customer customer)
    {
        return new CustomerModel
        {
            Id = customer.Id,
            Name = customer.Name,
            LastName = customer.LastName,
            Number = customer.Number,
            Email = customer.Email,
            CustomValidationField = customer.CustomValidationField,
            Photo = customer.PhotoString
        };
    }

    public static Domain.Customer ToDomain(this CustomerModel customerModel)
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