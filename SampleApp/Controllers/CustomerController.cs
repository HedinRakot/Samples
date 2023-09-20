using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Application;
using SampleApp.Models.Mapping;

namespace SampleApp.Controllers;

public class CustomerController : Controller
{
    private CustomerRepository _customerRepository;
    private IMapping<Domain.Customer, CustomerModel> _customerMapping;
    public CustomerController(CustomerRepository customerRepository, 
        IMapping<Domain.Customer, CustomerModel> customerMapping)
    {
        _customerRepository = customerRepository;
        _customerMapping = customerMapping;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var models = new List<CustomerModel>();
        foreach (var item in _customerRepository.Customers)
        {
            models.Add(CustomerModelMapping.Map(item));
            //models.Add(item.ToModel());
            //models.Add(_customerMapping.Map(item));
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

            var newCustomer = CustomerModelMapping.Map(model);
            //var newCustomer = _customerMapping.Map(model);
            newCustomer.Id = lastId + 1;

            _customerRepository.Customers.Add(newCustomer);

            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    [HttpGet]
    public IActionResult Edit(long id)
    {
        var customer = _customerRepository.Customers.FirstOrDefault(x => x.Id == id);
        return View(CustomerModelMapping.Map(customer));
        //return View(_customerMapping.Map(customer));
    }

    [HttpPost]
    public IActionResult Edit(CustomerModel model)
    {
        if (ModelState.IsValid)
        {
            var customer = _customerRepository.Customers.FirstOrDefault(x => x.Id == model.Id);
            var index = _customerRepository.Customers.IndexOf(customer);
            _customerRepository.Customers.Remove(customer);

            var editedCustomer = CustomerModelMapping.Map(model);
            //var editedCustomer = model.ToDomain();
            _customerRepository.Customers.Insert(index, editedCustomer);
            
            //customer = _customerMapping.Map(model);

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