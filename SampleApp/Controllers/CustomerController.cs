using Microsoft.AspNetCore.Mvc;
using SampleApp.Database;
using SampleApp.Domain.Repositories;
using SampleApp.Models;
using SampleApp.Models.Mapping;

namespace SampleApp.Controllers;

public class CustomerController : Controller
{
    private IMapping<Domain.Customer, CustomerModel> _customerMapping;
    private readonly ISqlUnitOfWork _unitOfWork;

    private readonly ICustomerRepository _dbCustomerRepository;

    public CustomerController(
        IMapping<Domain.Customer, CustomerModel> customerMapping,
        ICustomerRepository dbCustomerRepository,
        ISqlUnitOfWork unitOfWork)
    {
        _customerMapping = customerMapping;
        _dbCustomerRepository = dbCustomerRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var models = new List<CustomerModel>();

        foreach (var item in _dbCustomerRepository.GetCustomers())
        {
            var addresses = _dbCustomerRepository.Addresses(item.Id);
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

            var password = newCustomer.EncodePassword(newCustomer.Password);
            newCustomer.Password = password;

            _dbCustomerRepository.AddCustomer(newCustomer);

            return RedirectToAction(nameof(Details), new { id = newCustomer.Id });
        }

        return View();
    }

    //[HttpGet]
    //public IActionResult Edit(long id)
    //{
    //    var customer = _memoryCustomerRepository.Customers.FirstOrDefault(x => x.Id == id);
    //    return View(CustomerModelMapping.Map(customer));
    //    //return View(_customerMapping.Map(customer));
    //}

    //[HttpPost]
    //public IActionResult Edit(CustomerModel model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        var customer = _memoryCustomerRepository.Customers.FirstOrDefault(x => x.Id == model.Id);
    //        var index = _memoryCustomerRepository.Customers.IndexOf(customer);
    //        _memoryCustomerRepository.Customers.Remove(customer);

    //        var editedCustomer = CustomerModelMapping.Map(model);
    //        //var editedCustomer = model.ToDomain();
    //        _memoryCustomerRepository.Customers.Insert(index, editedCustomer);

    //        //customer = _customerMapping.Map(model);

    //        return RedirectToAction(nameof(Index));
    //    }

    //    return View();
    //}

    //[HttpDelete]
    //public IActionResult Delete(long id)
    //{
    //    var customer = _memoryCustomerRepository.Customers.FirstOrDefault(x => x.Id == id);

    //    if (customer != null)
    //    {
    //        _memoryCustomerRepository.Customers.Remove(customer);
    //    }
    //    else
    //    {
    //        return BadRequest(new { errorMessage = "Element was not found" });
    //    }

    //    return Ok();
    //}

    [HttpGet]
    public IActionResult Details(long id)
    {
        var customer = _dbCustomerRepository.GetCustomer(id);
        return View(customer.ToModel());
    }

    [HttpGet]
    public IActionResult UploadPhoto(long id)
    {
        return View(new CustomerModel
        {
            Id = id
        });
    }

    [HttpPost]
    public IActionResult UploadPhoto(
        [FromForm] IFormFile photoFile,
        [FromRoute] long id)
    {
        if (photoFile != null && photoFile.Length != 0)
        {
            var memoryStream = new MemoryStream();
            photoFile.CopyTo(memoryStream);

            var customer = _unitOfWork.CustomerRepository.GetCustomer(id);

            var str = Convert.ToBase64String(memoryStream.ToArray());
            byte[] bytes = memoryStream.ToArray();

            customer.PhotoString = str;
            customer.PhotoBinary = bytes;

            _unitOfWork.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = id });
        }

        return View();
    }
}