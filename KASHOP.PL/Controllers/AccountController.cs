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
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _AuthenticationService.RegisterAsync(request);
            return Ok(result);
        }
    }
}
