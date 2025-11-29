using Microsoft.AspNetCore.Mvc;
using Order.Api.Dtos;
using Order.Api.Services;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateRequestDto orderCreateRequestDto)
        {
            return Ok(await _orderService.Create(orderCreateRequestDto));
        }
    }
}
