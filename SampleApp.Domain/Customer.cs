namespace SampleApp.Domain;

public class Customer
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int Number { get; set; }
    public string CustomValidationField { get; set; }
}