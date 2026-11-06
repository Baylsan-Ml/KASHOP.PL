using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Enums;
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
        private readonly IOrderService _orderService;
        public OrdersController(ICheckoutService checkoutService, IOrderService orderService)
        {
            _checkoutService = checkoutService;
            _orderService = orderService;
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
        [HttpGet("")]
        public async Task<IActionResult> GetMyOrders()
        {
            var result = await _orderService.GetUserOrdersAsync(CurrentUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMyOrderDetails(int id)
        {
            var result = await _orderService.GetUserOrderDetailsAsync(id, CurrentUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // Admin 
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderStatusEnum? status = OrderStatusEnum.Pending)
        {
            var result = await _orderService.GetAllOrdersAsync(status);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
