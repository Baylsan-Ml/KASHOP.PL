using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>>GetAllCategoriesAsync();
        Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request);
        Task<CategoryResponse> GetCategory(Expression<Func<Category, bool>> filter);
        Task<CategoryResponse> UpdateCategoryAsync(CategoryRequest request);
        Task <bool> DeleteCategoryAsync(int id);
    }
}
