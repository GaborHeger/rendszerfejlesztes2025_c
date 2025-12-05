using Microsoft.AspNetCore.Mvc;
using webshop_barbie.Service.Interfaces;
using webshop_barbie.DTOs;

namespace webshop_barbie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts([FromQuery] string? category = null)
        {
            var products = await _productService.GetAllProductsAsync(category);
            return Ok(products);
        }
    }
}
