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
    public interface IProductService
    {
        Task<Result<ProductResponse>> CreateProductAsync(ProductRequest request);
        Task<Result<List<ProductResponse>>>GetAllProductsAsync();
        Task<Result<ProductResponse>> GetProductAsyns(Expression<Func<Product, bool>> filter);
        Task<Result<ProductResponse>> UpdateProductAsync(int id, ProductRequest request);
        Task<Result<bool>> DeleteProduct(int id);
    }
}
