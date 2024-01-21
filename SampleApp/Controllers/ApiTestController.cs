using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using SampleApp.Authentication;
using System.Text.Json;
using SampleApp.Application;

namespace SampleApp.Controllers
{
    public class ApiTestController : Controller
    {
        private readonly IOrderService _orderService;

        public ApiTestController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationScheme.DefaultScheme)]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrders();

            //TODO map from Domain.Order to OrderModel


            return Ok(orders);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationScheme.DefaultScheme)]
        public async Task<IActionResult> Add(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var newOrder = new Domain.Order
                {
                    CustomerId = model.CustomerId,
                    OrderNumber = model.OrderNumber
                };

                newOrder = await _orderService.AddOrder(newOrder);

                return Ok(newOrder);
            }

            return BadRequest();
        }
    }
}