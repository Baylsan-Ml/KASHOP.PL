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
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository) {

            _categoryRepository=categoryRepository;
        }

         public async Task<Result<CategoryResponse>> CreateCategoryAsync(CategoryRequest request)
        {
            try
            {
                var category = request.Adapt<Category>();
                await _categoryRepository.CreateAsync(category);
                return new Result<CategoryResponse>
                {
                    Success = true,
                    Message = "Success",
                };
            }catch (Exception ex) 
            {
                return new Result<CategoryResponse>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                };
            }
        }

        public async Task<Result<List<CategoryResponse>>> GetAllCategoriesAsync()
        {
            try
            {
                var lang = CultureInfo.CurrentUICulture.Name;
                var categories = await _categoryRepository.GetAllAsync(
                    new string[] { nameof(Category.Translations), "CreatedBy" }
                    );
                return new Result<List<CategoryResponse>> {
                    Success = true,
                    Message = "Success",
                    Data = categories.Adapt<List<CategoryResponse>>()
                };
            } catch (Exception ex) {
                return new Result<List<CategoryResponse>>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                };
            }
        }
        public async Task<Result<CategoryResponse>> GetCategory(Expression<Func<Category, bool>> filter)
        {
            try
            {
                var category = await _categoryRepository.GetOne(filter, new string[] { nameof(Category.Translations) });
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

            }catch (Exception ex)
            {
                return new Result<CategoryResponse>()
                {
                    Success = false,
                    Message = ex.InnerException.Message
                };
            }
        }
        public async Task<Result<CategoryResponse>> UpdateCategoryAsync(int id, CategoryRequest request)
        {
            try
            {
                var category = request.Adapt<Category>();
                category.Id = id;
                var updatedCategory = await _categoryRepository.UpdateAsync(category);
                return new Result<CategoryResponse>()
                {
                    Success = true,
                    Message = "Success",
                    Data = updatedCategory.Adapt<CategoryResponse>()
                };
            }
            catch (Exception ex)
            {
                return new Result<CategoryResponse>()
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                };
            }
        }
        public async Task<Result<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetOne(c => c.Id == id);

                if (category == null)
                {
                    return new Result<bool>
                    {
                        Success = false,
                        Message = "Could Not Delete",
                        Data = false
                    };
                }
                var Deleted = await _categoryRepository.DeleteAsync(category);
                return new Result<bool>
                {
                    Success = Deleted,
                    Message = Deleted? "Success": "Failed to Delete Category",
                    Data = Deleted
                };
            }
            catch (Exception ex) 
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                    Data = false
                };

            }
        }
    }
}
