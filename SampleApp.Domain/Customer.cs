using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SampleApp.Domain;

public class Customer
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int Number { get; set; }
    public string CustomValidationField { get; set; }

    public virtual List<Order> Orders { get; set; }

    public virtual List<Address> Address { get; set; }

    public string? PhotoString { get; set; }
    public byte[]? PhotoBinary { get; set; }

    public string Password { get; set; }

    public string EncodePassword(string password)
    {
        var sha = SHA512.Create("SHA-512");
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        var result = Convert.ToBase64String(hash);

        return result;
    }

    public bool ValidatePassword(string password)
    {
        var sha = SHA512.Create("SHA-512");
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        var encodedPassword = Convert.ToBase64String(hash);

        return encodedPassword == Password;
    }
}