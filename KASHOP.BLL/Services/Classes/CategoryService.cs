using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Classes
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork) {

            _unitOfWork = unitOfWork;
        }

         public async Task<Result<CategoryResponse>> CreateCategoryAsync(CategoryRequest request)
        {
                var category = request.Adapt<Category>();
                await _unitOfWork.CategoryRepository.CreateAsync(category);
            await _unitOfWork.CompleteAsync();
                return new Result<CategoryResponse>
                {
                    Success = true,
                    Message = "Success",
                };
        }

        public async Task<Result<List<CategoryResponse>>> GetAllCategoriesAsync()
        {

                var lang = CultureInfo.CurrentUICulture.Name;
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync(
                    new string[] { nameof(Category.Translations), "CreatedBy" }
                    );
                return new Result<List<CategoryResponse>> {
                    Success = true,
                    Message = "Success",
                    Data = categories.Adapt<List<CategoryResponse>>()
                };
        }
        public async Task<Result<CategoryResponse>> GetCategory(Expression<Func<Category, bool>> filter)
        {
                var category = await _unitOfWork.CategoryRepository.GetOne(filter, new string[] { nameof(Category.Translations) });
                if (category == null)
                {
                    return new Result<CategoryResponse>()
                    {
                        Success = false,
                        Message = "Category not found",
                    };
                }
                return new Result<CategoryResponse>()
                {
                    Success = true,
                    Message = "Success",
                    Data = category.Adapt<CategoryResponse>()
                };
        }
        public async Task<Result<CategoryResponse>> UpdateCategoryAsync(int id, CategoryRequest request)
        {
                var category = request.Adapt<Category>();
                category.Id = id;
                var updatedCategory = await _unitOfWork.CategoryRepository.UpdateAsync(category);
                return new Result<CategoryResponse>()
                {
                    Success = true,
                    Message = "Success",
                    Data = updatedCategory.Adapt<CategoryResponse>()
                };
           
        }
        public async Task<Result<bool>> DeleteCategoryAsync(int id)
        {
            
                var category = await _unitOfWork.CategoryRepository.GetOne(c => c.Id == id);

                if (category == null)
                {
                    return new Result<bool>
                    {
                        Success = false,
                        Message = "Could Not Delete",
                        Data = false
                    };
                }
                 _unitOfWork.CategoryRepository.Delete(category);
                  var affectedRows = await _unitOfWork.CompleteAsync();
                return new Result<bool>
                {
                    Success = affectedRows > 0,
                    Message = affectedRows > 0 ? "Success": "Failed to Delete Category",
                };
        }
    }
}
