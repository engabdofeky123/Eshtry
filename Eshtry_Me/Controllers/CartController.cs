using Eshtry_Me.DTOs;
using Eshtry_Me.Repositories.Implementation;
using Eshtry_Me.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eshtry_Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IAddressRepository _addressRepository;

        public CartController(ICartRepository repo, IAddressRepository addresses)
        {
            _cartRepository = repo;
            _addressRepository = addresses;
        }

        [HttpGet("my-cart")]
        public async Task<IActionResult> GetMyCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartRepository.GetCartByUserId(userId);
            if(cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [Authorize]
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart(AddToCartModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _cartRepository.AddToCartAsync(model, userId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("addresses")]
        public async Task<IActionResult> GetAddresses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _getAllAddresses.GetAllAddressesAsync(userId);
            return Ok(result);
        }

        [HttpPost("add-new-address")]
        public async Task<IActionResult> AddNewAddress(AddressDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _addressRepository.AddNewAddressAsync(model, userId);
            return Ok(result);
        }
    }
}
