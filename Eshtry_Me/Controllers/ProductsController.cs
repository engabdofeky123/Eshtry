using Eshtry_Me.DTOs;
using Eshtry_Me.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eshtry_Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsController(IProductsRepository repo)
        {
            _productsRepository = repo;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productsRepository.GetProductById(id);

            if (product == null)
                return NotFound("Product not found.");
            return Ok(product);
        }


        [Authorize]
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart(AddToCartModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _productsRepository.AddToCartAsync(model, userId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    
    
    }
}
