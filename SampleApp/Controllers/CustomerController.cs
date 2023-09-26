using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Application;
using SampleApp.Models.Mapping;
using SampleApp.Domain.Repositories;

namespace SampleApp.Controllers;

public class CustomerController : Controller
{
    private CustomerRepository _memoryCustomerRepository;
    private IMapping<Domain.Customer, CustomerModel> _customerMapping;
    
    private readonly ICustomerRepository _dbCustomerRepository;

    public CustomerController(CustomerRepository customerRepository, 
        IMapping<Domain.Customer, CustomerModel> customerMapping,
        ICustomerRepository dbCustomerRepository)
    {
        _memoryCustomerRepository = customerRepository;
        _customerMapping = customerMapping;
        _dbCustomerRepository = dbCustomerRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var models = new List<CustomerModel>();
        foreach (var item in _memoryCustomerRepository.Customers)
        {
            models.Add(CustomerModelMapping.Map(item));
            //models.Add(item.ToModel());
            //models.Add(_customerMapping.Map(item));
        }

        foreach(var item in _dbCustomerRepository.GetCustomers())
        {
            models.Add(item.ToModel());
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
            var newCustomer = CustomerModelMapping.Map(model);

            //var lastId = _memoryCustomerRepository.Customers.Count != 0 ? _memoryCustomerRepository.Customers.Max(x => x.Id) : 0;

            ////var newCustomer = _customerMapping.Map(model);
            //newCustomer.Id = lastId + 1;

            //_memoryCustomerRepository.Customers.Add(newCustomer);

            _dbCustomerRepository.AddCustomer(newCustomer);

            return RedirectToAction(nameof(Details), new { id = newCustomer.Id });
        }

        return View();
    }

    [HttpGet]
    public IActionResult Edit(long id)
    {
        var customer = _memoryCustomerRepository.Customers.FirstOrDefault(x => x.Id == id);
        return View(CustomerModelMapping.Map(customer));
        //return View(_customerMapping.Map(customer));
    }

    [HttpPost]
    public IActionResult Edit(CustomerModel model)
    {
        if (ModelState.IsValid)
        {
            var customer = _memoryCustomerRepository.Customers.FirstOrDefault(x => x.Id == model.Id);
            var index = _memoryCustomerRepository.Customers.IndexOf(customer);
            _memoryCustomerRepository.Customers.Remove(customer);

            var editedCustomer = CustomerModelMapping.Map(model);
            //var editedCustomer = model.ToDomain();
            _memoryCustomerRepository.Customers.Insert(index, editedCustomer);
            
            //customer = _customerMapping.Map(model);

            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    [HttpDelete]
    public IActionResult Delete(long id)
    {
        var customer = _memoryCustomerRepository.Customers.FirstOrDefault(x => x.Id == id);

        if (customer != null)
        {
            _memoryCustomerRepository.Customers.Remove(customer);
        }
        else
        {
            return BadRequest(new { errorMessage = "Element was not found" });
        }

        return Ok();
    }

    [HttpGet]
    public IActionResult Details(long id)
    {
        var customer = _dbCustomerRepository.GetCustomer(id);
        return View(CustomerModelMapping.Map(customer));
    }
}