﻿using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Domain.Repositories;
using SampleApp.Models.Mapping;
using SampleApp.Database;

namespace SampleApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderHistoryRepository _orderHistoryRepository;
        private readonly ISqlUnitOfWork _unitOfWork;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IOrderHistoryRepository orderHistoryRepository,
            ISqlUnitOfWork unitOfWork,
            ILogger<OrderController> logger)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _orderHistoryRepository = orderHistoryRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogCritical("Test Critical");
            _logger.LogError("Test Error");
            _logger.LogWarning("Test Warning");
            _logger.LogInformation("Test Info message");
            _logger.LogDebug("Test Debug");
            _logger.Log(LogLevel.Trace, "Test Trace");

            var models = new List<OrderModel>();
            foreach (var item in _orderRepository.GetOrderWithCustomers())
            {
                models.Add(new OrderModel
                {
                    Id = item.Id,
                    CustomerId = item.Customer.Id,
                    OrderNumber = item.OrderNumber
                });
            }

            return View(models);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new OrderModel()
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
        public IActionResult Add(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var newOrder = new Domain.Order
                {
                    CustomerId = model.CustomerId,
                    OrderNumber = model.OrderNumber
                };

                _unitOfWork.OrderRepository.AddOrder(newOrder);
                _unitOfWork.OrderHistoryRepository.AddOrderHistory(newOrder);
                _unitOfWork.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            model.Customers = GetCustomerModels();
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var order = _orderRepository.GetById(id);
            return View(new OrderModel
            {
                Id = order.Id,
                CustomerId = order.Customer.Id,
                OrderNumber = order.OrderNumber,
                Customers = GetCustomerModels()
            });
        }

        [HttpPost]
        public IActionResult Edit(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var order = _orderRepository.GetByIdWithHistory(model.Id);
                order.CustomerId = model.CustomerId;
                order.OrderNumber = model.OrderNumber;

                _orderRepository.UpdateOrder(order);

                return RedirectToAction(nameof(Index));
            }

            model.Customers = GetCustomerModels();
            return View(model);
        }

        //[HttpDelete]
        //public IActionResult Delete(long id)
        //{
        //    var order = _orderRepository.Orders.FirstOrDefault(x => x.Id == id);

        //    if (order != null)
        //    {
        //        _orderRepository.Orders.Remove(order);
        //    }
        //    else
        //    {
        //        return BadRequest(new { errorMessage = "Element was not found" });
        //    }

        //    return Ok();
        //}
    }
}