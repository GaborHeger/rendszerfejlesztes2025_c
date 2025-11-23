using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webshop_barbie.DTOs;
using webshop_barbie.DTOs.Requests;
using webshop_barbie.Service.Interfaces;

namespace webshop_barbie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [Authorize]
        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateOrder(int userId, [FromBody] OrderRequestDTO orderRequest)
        {
            var result = await _orderService.CreateOrderAsync(userId, orderRequest);
            return Ok(result);
        }
    }
}
