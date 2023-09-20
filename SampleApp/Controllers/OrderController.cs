using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Application;

namespace SampleApp.Controllers
{
    public class OrderController : Controller
    {
        private OrderRepository _orderRepository;
        public OrderController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var models = new List<OrderModel>();
            foreach (var item in _orderRepository.Orders)
            {
                models.Add(new OrderModel
                {
                    Id = item.Id,
                    CustomerName = item.Customer.Name
                });
            }

            return View(models);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new OrderModel());
        }

        [HttpPost]
        public IActionResult Add(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var lastId = _orderRepository.Orders.Count != 0 ? _orderRepository.Orders.Max(x => x.Id) : 0;

                _orderRepository.Orders.Add(new Domain.Order
                {
                    Id = lastId + 1,
                    //Customer = 
                });

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var order = _orderRepository.Orders.FirstOrDefault(x => x.Id == id);
            return View(new OrderModel
            {
                Id = order.Id,
                CustomerName = order.Customer.Name,
            });
        }

        [HttpPost]
        public IActionResult Edit(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var order = _orderRepository.Orders.FirstOrDefault(x => x.Id == model.Id);
                //order.Customer = model.Name;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpDelete]
        public IActionResult Delete(long id)
        {
            var order = _orderRepository.Orders.FirstOrDefault(x => x.Id == id);

            if (order != null)
            {
                _orderRepository.Orders.Remove(order);
            }
            else
            {
                return BadRequest(new { errorMessage = "Element was not found" });
            }

            return Ok();
        }
    }
}