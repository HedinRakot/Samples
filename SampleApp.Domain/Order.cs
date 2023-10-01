namespace SampleApp.Domain;

public class Order
{
    public long Id { get; set; }
    public string OrderNumber { get; set; }
    public Customer Customer { get; set; }
}