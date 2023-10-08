namespace SampleApp.Domain;

public class Customer
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int Number { get; set; }
    public string CustomValidationField { get; set; }

    public virtual List<Order> Orders { get; set; }

    public virtual List<Address> Address { get; set; }

    public string? PhotoString { get; set; }
    public byte[]? PhotoBinary { get; set; }
}