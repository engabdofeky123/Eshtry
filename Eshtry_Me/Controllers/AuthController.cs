using Eshtry_Me.DTOs;
using Eshtry_Me.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eshtry_Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        private readonly IConfiguration _config;
        public AuthController(IAuth auth, IConfiguration config)
        {
            _authService = auth;
            _config = config;
        }

        // Standard email/password authentication endpoints

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result.IsAuthenticated == false || result is null)
                return Unauthorized("Invalid email or password");
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (result.IsAuthenticated == false || result is null)
                return BadRequest("User with this email already exists");
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
} 
