namespace SampleApp.Domain;

public class Coupon
{
    public long Id { get; set; }
    public int Discount { get; set; }
    public int? Count { get; set; }
    public int? AppliedCount { get; set; }
}