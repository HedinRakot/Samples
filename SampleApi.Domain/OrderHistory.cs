namespace SampleApi.Domain;

public class OrderHistory
{
    public long Id { get; set; }
    public virtual Order Order { get; set; }
    public long OrderId { get; set; }
    public string Changes { get; set; }
    public DateTimeOffset ChangeDate { get; set; }
}