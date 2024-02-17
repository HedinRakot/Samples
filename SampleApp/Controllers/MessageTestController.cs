using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleApp.Application;
using SampleApp.Authentication;

namespace SampleApp.Controllers
{
    public class MessageTestController : Controller
    {
        private readonly IMessageService _messageService;

        public MessageTestController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationScheme.DefaultScheme)]
        public async Task<IActionResult> Index()
        {
            await _messageService.SendOrder();

            return Ok();
        }
    }
}