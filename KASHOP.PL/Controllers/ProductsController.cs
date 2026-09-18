using KASHOP.BLL.Services.Classes;
using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using static System.Net.WebRequestMethods;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromForm] ProductRequest request)
        {
            var result = await _productService.CreateProductAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var result = await _productService.GetProductAsyns(p=> p.Id == id);
            return result.Success ? Ok(result) : NotFound(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] ProductRequest request)
        {
            var result = await _productService.UpdateProductAsync(id, request);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProduct(id);
            return result.Success ? Ok(result) : BadRequest(result);

        }
    }
}
