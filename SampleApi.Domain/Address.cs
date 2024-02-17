namespace SampleApi.Domain;

public class Address
{
    public long Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string HouseNumber { get; set; }
    public int Type { get; set; }

    public virtual List<Customer> Customers { get; set; }
}