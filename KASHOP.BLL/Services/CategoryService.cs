using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

         async Task<CategoryResponse> ICategoryService.CreateCategoryAsync(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            await _categoryRepository.CreateAsync(category);
            var response = category.Adapt<CategoryResponse>();
            return response;
        }

        public async Task<List<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync(
                new string[] { nameof(Category.Translations) }
                );
            var response = categories.Adapt<List<CategoryResponse>>();

            return response;
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            await _categoryRepository.UpdateAsync(category);
            var response = category.Adapt<CategoryResponse>();
            return response;
        }

        public async Task<CategoryResponse> GetCategory(Expression<Func<Category, bool>> filter)
        {
            var category = await _categoryRepository.GetOne(filter, new string[] { nameof(Category.Translations) });

            return category.Adapt<CategoryResponse>();
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetOne(c => c.Id == id);

            if (category == null)
                return false;

            return await _categoryRepository.DeleteAsync(category);
        }
    }
}
