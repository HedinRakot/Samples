using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SampleApi.Application;
using SampleApi.Domain;
using SampleApi.Domain.Repositories;
using SampleApi.Models;
using System.Security.Claims;

namespace SampleApi.Controllers;

public class LoginController : Controller
{
    private ICustomerRepository _customerRepository;
    public LoginController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return Ok(new LoginModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var customer = _customerRepository.GetCustomers().FirstOrDefault(x => x.Name == model.UserName);

                if (customer != null && customer.ValidatePassword(model.Password))
                {
                    await SignIn(customer);

                    return RedirectToAction(nameof(CustomerController.Index), nameof(SampleApi.Domain.Customer));
                }
                else
                {
                    ModelState.AddModelError("Model", "User doesnt exist or password wrong");
                }
            }
            catch (NullReferenceException exception)
            {
                ModelState.AddModelError("Model", "User doesnt exist");
                //TODO Logging
            }
            //catch(Exception exception)
            //{
            //    ModelState.AddModelError("Model", "Unexpected error occured. Please try again later..");
            //TODO Logging
            //}
        }

        return Ok(model);
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

