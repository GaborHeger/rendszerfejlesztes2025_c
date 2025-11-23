using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webshop_barbie.DTOs;
using webshop_barbie.DTOs.Requests;
using webshop_barbie.Service.Interfaces;

namespace webshop_barbie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult<CartDTO>> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            return Ok(cart);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CartDTO>> AddItemToCart(int userId, [FromBody] UpdateCartItemRequestDTO request)
        {
            if (request == null)
                return BadRequest("Request body is null.");

            var updatedCart = await _cartService.AddItemToCartAsync(userId, request.ProductId, request.Quantity);
            return Ok(updatedCart);
        }

        [Authorize]
        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult<CartDTO>> RemoveItem(int userId, int productId)
        {
            var updatedCart = await _cartService.RemoveItemFromCartAsync(userId, productId);
            return Ok(updatedCart);
        }

        [Authorize]
        [HttpPut("{userId}/items")]
        public async Task<ActionResult<CartDTO>> UpdateCartItemQuantityAsync(int userId, [FromBody] UpdateCartItemRequestDTO dto)
        {
            var updatedCart = await _cartService.UpdateCartItemQuantityAsync(userId, dto.ProductId, dto.Quantity);
            return Ok(updatedCart);
        }

        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<ActionResult<string>> ClearCartAsync(int userId)
        {

            var message = await _cartService.ClearCartAsync(userId);
            return Ok(message);
        }

    }
}
