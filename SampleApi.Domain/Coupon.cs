namespace SampleApi.Domain;

public class Coupon
{
    public long Id { get; set; }
    public string Code { get; set; }
    public int Discount { get; set; }
    public int? Count { get; set; }
    public int? AppliedCount { get; set; }

    public byte[] Version { get; private set; }
}