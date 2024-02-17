using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace SampleApi.Authentication;

public class ApiKeyRequirement : IAuthorizationRequirement
{
}

public class ApiKeyAuthorizationHandler : AuthorizationHandler<ApiKeyRequirement>
{
    //private readonly IHttpContextAccessor _httpContextAccessor;
    public ApiKeyAuthorizationHandler()//IHttpContextAccessor httpContextAccessor)
    {
        //_httpContextAccessor = httpContextAccessor;
    }
    
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, ApiKeyRequirement requirement)
    {
        //var _httpContextAccessor = context.
        //var apiKey = _httpContextAccessor?.HttpContext?.Request.Headers.FirstOrDefault(o => o.Key == "x-api-key");

        //if (apiKey != null && apiKey.ToString() == "1234567890")
        //{
        //    var claims = new[] {
        //        new Claim(ClaimTypes.Name, "SystemAccount"),
        //    };

        //    var identity = new ClaimsIdentity(claims, ApiKeyAuthenticationScheme.DefaultScheme);
        //    //var identities = new List<ClaimsIdentity> { identity }; // für mehrere Identities
        //    //var principal = new ClaimsPrincipal(identities);

        //    var principal = new ClaimsPrincipal(identity);
        //    var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationScheme.DefaultScheme);

        //    context.Succeed(requirement);
        //}
        //else
        //{
        //    context.Fail();
        //}

        return Task.CompletedTask;
    }
}
