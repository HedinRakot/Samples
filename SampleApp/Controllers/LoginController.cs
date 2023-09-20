using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SampleApp.Application;
using SampleApp.Domain;
using SampleApp.Models;
using System.Security.Claims;

namespace SampleApp.Controllers;

public class LoginController : Controller
{
    private CustomerRepository _customerRepository;
    public LoginController(CustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View(new LoginModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var customer = _customerRepository.Customers.FirstOrDefault(x => x.Name == model.UserName);

            await SignIn(customer);

            return RedirectToAction(nameof(CustomerController.Index), nameof(SampleApp.Domain.Customer));
        }

        return View("~/Views/Login/SignIn.cshtml", model);
    }

    protected async Task SignIn(Customer customer)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, customer.Name),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties()
        {
            IsPersistent = true,
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.Now.AddDays(1),
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
    }

    [HttpGet]
    public async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(SignIn));
    }
}

