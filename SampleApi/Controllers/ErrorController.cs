using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleApi.Controllers;

[AllowAnonymous]
public class ErrorController : Controller
{
    public IActionResult UnexpectedError()
    {
        return Ok();
    }
}