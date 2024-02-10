namespace SampleApp.Domain.Settings;

public class ApiKeyAuthenticationOptions
{
    public const string SectionName = "Authentication";

    public List<ApiKeyUser> Users { get; set; }
}


public class ApiKeyUser
{
    public string ApiKey { get; set; }
    public string Owner { get; set; }
}