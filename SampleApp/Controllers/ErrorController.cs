using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Controllers;

[AllowAnonymous]
public class ErrorController : Controller
{
    public IActionResult UnexpectedError()
    {
        return View();
    }
}