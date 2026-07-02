using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository) {

            _categoryRepository=categoryRepository;
        }

        public CategoryResponse CreateCategory(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            _categoryRepository.Create(category);
            var response = category.Adapt<CategoryResponse>();
            return response;
        }

        public List<CategoryResponse> GetAllCategories()
        {
            var categories = _categoryRepository.GetAll();
            var response = categories.Adapt<List<CategoryResponse>>();

            return response;
        }
    }
}
