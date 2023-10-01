namespace SampleApp.Domain;

public class OrderHistory
{
    public long Id { get; set; }
    public Order Order { get; set; }
    public long OrderId { get; set; }
    public string Changes { get; set; }
    public DateTimeOffset ChangeDate { get; set; }
}