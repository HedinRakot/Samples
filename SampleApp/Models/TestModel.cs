using System.ComponentModel.DataAnnotations;

namespace SampleApp.Models;

public class TestModel
{
    public long Id { get; set; } = 0;

    [Required]
    public string Name { get; set; }

    [Required]
    public string LastName { get; set; }
}