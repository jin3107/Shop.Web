using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Web.DTOs.Auth;
using Shop.Web.Services.Interface;

namespace Shop.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SignInDTO signInDto)
        {
            try
            {
                var response = await _authService.RegisterUser(signInDto);
                if (response != null)
                {
                    return Ok(response);
                }
                return BadRequest("Registration failed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInDTO loginDto)
        {
            try
            {
                var response = await _authService.LoginUser(loginDto);
                if (response != null)
                {
                    return Ok(response);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
