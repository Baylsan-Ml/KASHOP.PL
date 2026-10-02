using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.Data;
using KASHOP.DAL.DTO;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : BaseApiController
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICartService _cartService;

        public CartsController(ApplicationDbContext context, IStringLocalizer<SharedResources> localizer, ICartService cartService)
        {
            _localizer = localizer; 
            _cartService = cartService;
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItemRequest request)
        {
            var result = await _cartService.AddToCart(CurrentUserId, request);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCart()
        {
            var result = await _cartService.GetCart(CurrentUserId);

            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
