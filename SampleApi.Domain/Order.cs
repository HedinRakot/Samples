namespace SampleApi.Domain;

public class Order
{
    public long Id { get; set; }
    public string OrderNumber { get; set; }
    public virtual Customer Customer { get; set; }
    public long CustomerId { get; set; }

    public virtual List<OrderHistory> History { get; set; } = new List<OrderHistory>();
}