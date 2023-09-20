using System.ComponentModel.DataAnnotations;

namespace SampleApp.Models;

public class OrderModel
{
    public long Id { get; set; } = 0;

    public string CustomerName { get; set; }
}