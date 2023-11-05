using System.ComponentModel.DataAnnotations;

namespace SampleApp.Models;

public class CookieOrderModel
{
    public long Id { get; set; } = 0;

    public long CustomerId { get; set; }
    public string OrderNumber { get; set; }

    public List<CustomerModel>? Customers { get; set; }
}