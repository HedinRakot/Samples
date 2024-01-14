using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Domain.Repositories;
using SampleApp.Database;
using SampleApp.Domain;
using Microsoft.AspNetCore.Authorization;
using SampleApp.Authentication;

namespace SampleApp.Controllers
{
    public class ApiTestController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public ApiTestController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        //[AllowAnonymous]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationSheme.DefaultSheme)]
        public IActionResult Index()
        {
            var apiKey = Request.Headers.FirstOrDefault(o => o.Key == "x-api-key");

            if(apiKey.Value != "1234567890")
            { 
                return BadRequest("NotAuth"); 
            }

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

            return Ok(models);
        }
    }
}