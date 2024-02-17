﻿using Microsoft.AspNetCore.Mvc;
using SampleApi.Models;
using SampleApi.Application;
using SampleApi.Models.Mapping;
using SampleApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using SampleApi.Database;
using System.Security.Cryptography;
using System.Text;

namespace SampleApi.Controllers;

public class CustomerController : Controller
{
    private CustomerRepository _memoryCustomerRepository;
    private IMapping<Domain.Customer, CustomerModel> _customerMapping;
    private readonly ISqlUnitOfWork _unitOfWork;

    private readonly ICustomerRepository _dbCustomerRepository;

    public CustomerController(CustomerRepository customerRepository,
        IMapping<Domain.Customer, CustomerModel> customerMapping,
        ICustomerRepository dbCustomerRepository,
        ISqlUnitOfWork unitOfWork)
    {
        _memoryCustomerRepository = customerRepository;
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

        return Ok(models);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return Ok(new CustomerModel());
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

        return Ok();
    }

    [HttpGet]
    public IActionResult Edit(long id)
    {
        var customer = _memoryCustomerRepository.Customers.FirstOrDefault(x => x.Id == id);
        return Ok(CustomerModelMapping.Map(customer));
        //return Ok(_customerMapping.Map(customer));
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

        return Ok();
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
        return Ok(customer.ToModel());
    }

    [HttpGet]
    public IActionResult UploadPhoto(long id)
    {
        return Ok(new CustomerModel
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

        return Ok();
    }
}