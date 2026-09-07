using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<List<CategoryResponse>>>GetAllCategoriesAsync();
        Task<Result<CategoryResponse>> CreateCategoryAsync(CategoryRequest request);
        Task<Result<CategoryResponse>> GetCategory(Expression<Func<Category, bool>> filter);
        Task<Result<CategoryResponse>> UpdateCategoryAsync(int id, CategoryRequest request);
        Task<Result<bool>> DeleteCategoryAsync(int id);
    }
}
