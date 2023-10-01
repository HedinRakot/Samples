namespace SampleApp.Domain;

public class Order
{
    public long Id { get; set; }
    public string OrderNumber { get; set; }
    public Customer Customer { get; set; }
    public long CustomerId { get; set; }

    public List<OrderHistory> History { get; set; } = new List<OrderHistory>();
}