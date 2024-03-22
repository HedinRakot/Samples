using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace SampleApp.Authentication;

internal class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) :
        base(options, logger, encoder, clock)
    {

    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var apiKey = Request.Headers.FirstOrDefault(o => o.Key == "x-api-key");

        //TODO if (apiKey.Value == "1234567890")
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, "SystemAccount"),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var identities = new List<ClaimsIdentity> { identity }; // für mehrere Identities
            //var principal = new ClaimsPrincipal(identities);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationScheme.DefaultScheme);

            return AuthenticateResult.Success(ticket);
        }

        return AuthenticateResult.Fail("No Api key or key is wrong");
    }
}
