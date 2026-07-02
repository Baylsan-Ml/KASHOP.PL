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
         List<CategoryResponse> GetAllCategories();
        CategoryResponse CreateCategory(CategoryRequest request);
    }
}
