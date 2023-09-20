using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Application;

namespace SampleApp.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerRepository _customerRepository;
        public CustomerController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var models = new List<CustomerModel>();
            foreach (var item in _customerRepository.Customers)
            {
                models.Add(new CustomerModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    LastName = item.LastName,
                });
            }

            return View(models);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new CustomerModel());
        }

        [HttpPost]
        public IActionResult Add(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                var lastId = _customerRepository.Customers.Count != 0 ? _customerRepository.Customers.Max(x => x.Id) : 0;

                _customerRepository.Customers.Add(new Domain.Customer
                {
                    Id = lastId + 1,
                    Name = model.Name,
                    LastName = model.LastName,
                });

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var customer = _customerRepository.Customers.FirstOrDefault(x => x.Id == id);
            return View(new CustomerModel
            {
                Id = customer.Id,
                Name = customer.Name,
                LastName = customer.LastName,
            });
        }

        [HttpPost]
        public IActionResult Edit(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = _customerRepository.Customers.FirstOrDefault(x => x.Id == model.Id);
                customer.Name = model.Name;
                customer.LastName = model.LastName;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpDelete]
        public IActionResult Delete(long id)
        {
            var customer = _customerRepository.Customers.FirstOrDefault(x => x.Id == id);

            if (customer != null)
            {
                _customerRepository.Customers.Remove(customer);
            }
            else
            {
                return BadRequest(new { errorMessage = "Element was not found" });
            }

            return Ok();
        }
    }
}