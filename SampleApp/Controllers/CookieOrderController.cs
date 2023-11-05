using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Domain.Repositories;
using SampleApp.Models.Mapping;
using System.Text.Json;

namespace SampleApp.Controllers
{
    public class CookieOrderController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CookieOrderController> _logger;

        public CookieOrderController(
            ICustomerRepository customerRepository,
            ILogger<CookieOrderController> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var models = new List<CookieOrderModel>();

            var cookieValue = Request.Cookies["myCookieOrders"];
            if(!String.IsNullOrWhiteSpace(cookieValue))
            {
                models = JsonSerializer.Deserialize<List<CookieOrderModel>>(cookieValue);
            }

            return View(models);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new CookieOrderModel()
            {
                Customers = GetCustomerModels()
            };

            return View(model);
        }

        private List<CustomerModel> GetCustomerModels()
        {
            var customers = _customerRepository.GetCustomers();
            var customerModels = new List<CustomerModel>();
            foreach (var customer in customers)
            {
                customerModels.Add(customer.ToModel());
            }

            return customerModels;
        }

        [HttpPost]
        public IActionResult Add(CookieOrderModel model)
        {
            if (ModelState.IsValid)
            {
                var models = new List<CookieOrderModel>();
                var cookieValue = Request.Cookies["myCookieOrders"];
                if (!String.IsNullOrWhiteSpace(cookieValue))
                {
                    models = JsonSerializer.Deserialize<List<CookieOrderModel>>(cookieValue);
                }

                model.Id = models.Count > 0 ? models.Max(o => o.Id) + 1 : 1;
                models.Add(model);

                var newCookieValue = JsonSerializer.Serialize(models);
                var options = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(6),
                };

                Response.Cookies.Append("myCookieOrders", newCookieValue, options);

                return RedirectToAction(nameof(Index));
            }

            model.Customers = GetCustomerModels();
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var models = new List<CookieOrderModel>();
            var cookieValue = Request.Cookies["myCookieOrders"];
            if (!String.IsNullOrWhiteSpace(cookieValue))
            {
                models = JsonSerializer.Deserialize<List<CookieOrderModel>>(cookieValue);
            }

            var model = models.FirstOrDefault(o => o.Id == id);
            model.Customers = GetCustomerModels();

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(CookieOrderModel model)
        {
            if (ModelState.IsValid)
            {
                var models = new List<CookieOrderModel>();
                var cookieValue = Request.Cookies["myCookieOrders"];
                if (!String.IsNullOrWhiteSpace(cookieValue))
                {
                    models = JsonSerializer.Deserialize<List<CookieOrderModel>>(cookieValue);
                }

                var updatedModel = models.FirstOrDefault(o => o.Id == model.Id);
                updatedModel.CustomerId = model.CustomerId;
                updatedModel.OrderNumber = model.OrderNumber;

                var newCookieValue = JsonSerializer.Serialize(models);
                var options = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(6),
                };

                Response.Cookies.Append("myCookieOrders", newCookieValue, options);

                return RedirectToAction(nameof(Index));
            }

            model.Customers = GetCustomerModels();
            return View(model);
        }

        //TODO
        //[HttpDelete]
        //public IActionResult Delete(long id)
        //{
        //}
    }
}