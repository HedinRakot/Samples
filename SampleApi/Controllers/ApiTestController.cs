using Microsoft.AspNetCore.Mvc;
using SampleApi.Models;
using SampleApi.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using SampleApi.Authentication;

namespace SampleApi.Controllers
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
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationScheme.DefaultScheme)]
        public IActionResult Index()
        {
            //var apiKey = Request.Headers.FirstOrDefault(o => o.Key == "x-api-key");

            //if(apiKey.Value != "1234567890")
            //{ 
            //    return Unauthorized(); 
            //}

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