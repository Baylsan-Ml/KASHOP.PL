using KASHOP.BLL.Services;
using KASHOP.DAL.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _AuthenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _AuthenticationService = authenticationService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _AuthenticationService.RegisterAsync(request);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _AuthenticationService.LoginAsync(request);
            return Ok(result);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
        {
            var result = await _AuthenticationService.ConfirnmEmail(request);
            if (!result) return BadRequest();
            return Ok("Email confirmed successfully.");
        }
    }
}
