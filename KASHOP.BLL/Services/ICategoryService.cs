using KASHOP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>>GetAllCategoriesAsync();
        Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request);
    }
}
