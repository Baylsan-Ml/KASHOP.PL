using KASHOP.BLL.Services;
using KASHOP.DAL;
using KASHOP.DAL.Data;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using KASHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICategoryService _categoryService;
        public CategoriesController(ApplicationDbContext context, IStringLocalizer<SharedResources> localizer, ICategoryService categoryService)
        {
            _localizer = localizer;
            _categoryService = categoryService;
        }
       
        [HttpGet("")]
        public ActionResult Index()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(new { _localizer["Success"].Value, categories });
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var response = _categoryService.CreateCategory(request);


            return Ok();
        }
    }

}
