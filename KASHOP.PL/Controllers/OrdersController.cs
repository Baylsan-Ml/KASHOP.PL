using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly ICheckoutService _checkoutService;
        public OrdersController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            var result = await _checkoutService.Checkout(CurrentUserId, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("payment/success")]
        [AllowAnonymous]
        public async Task<IActionResult> PaymentSuccess(string sessionId)
        {
            var result = await _checkoutService.HandleSuccess(sessionId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("payment/cancel")]
        [AllowAnonymous]
        public async Task<IActionResult> Cancel(string sessionId)
        {
            var result = await _checkoutService.HandleSuccess(sessionId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
