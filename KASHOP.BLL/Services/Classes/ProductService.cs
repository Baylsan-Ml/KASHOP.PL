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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _ProductRepository;
        private readonly IFileService _FileService;

        public ProductService(IProductRepository ProductRepository, IFileService FileService)
        {

            _ProductRepository = ProductRepository;
            _FileService = FileService;
        }

        public async Task<Result<ProductResponse>> CreateProductAsync(ProductRequest request)
        {
            try
            {
                if (request.MainImage is null)
                {
                    return new Result<ProductResponse>
                    {
                        Success = false,
                        Message = "Main image is required",
                    };
                }
                var UploadResult = await _FileService.UploadAsync(request.MainImage);

                if (!UploadResult.Success)
                {
                    return new Result<ProductResponse>
                    {
                        Success = false,
                        Message = UploadResult.Message,
                    };
                }
                var product = request.Adapt<Product>();
                product.MainImage = UploadResult.Data;
                await _ProductRepository.CreateAsync(product);

                return new Result<ProductResponse>
                {
                    Success = true,
                    Message = "Product Created Successfully1!",
                    Data = product.Adapt<ProductResponse>()
                };
            }
            catch (Exception ex)
            {
                return new Result<ProductResponse>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                };
            }
        }
        public async Task<Result<List<ProductResponse>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _ProductRepository.GetAllAsync(
                    new string[] { nameof(Product.Translations), nameof(Product.Category) });
                return new Result<List<ProductResponse>>
                {
                    Success = true,
                    Message = "Success!",
                    Data = products.Adapt<List<ProductResponse>>()
                };
            }
            catch (Exception ex)
            {
                return new Result<List<ProductResponse>>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                };
            }
        }

        public async Task<Result<ProductResponse>> GetProductAsyns(Expression<Func<Product, bool>> filter)
        {
            try
            {
                var product = await _ProductRepository.GetOne(filter, new string[] { nameof(Product.Translations), nameof(Product.Category) });
                if (product == null)
                {
                    return new Result<ProductResponse>
                    {
                        Success = false,
                        Message = "Product Not Found :("
                    };
                }
                return new Result<ProductResponse>
                {
                    Success = true,
                    Message = "Success!",
                    Data = product.Adapt<ProductResponse>()
                };
            }
            catch (Exception ex)
            {
                return new Result<ProductResponse>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                };
            }
        }

        public async Task<Result<ProductResponse>> UpdateProductAsync(int id, ProductRequest request)
        {
            try
            {
                var product = request.Adapt<Product>();
                product.Id = id;
                var UpdatedProduct = await _ProductRepository.UpdateAsync(product);
                return new Result<ProductResponse>
                {
                    Success = true,
                    Message = "Product Updated Successfully!",
                    Data = UpdatedProduct.Adapt<ProductResponse>()
                };
            }
            catch (Exception ex)
            {
                return new Result<ProductResponse>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                };
            }
        }

        public async Task<Result<bool>> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _ProductRepository.GetOne(p => p.Id == id);
                if (product is null)
                {
                    return new Result<bool>
                    {
                        Success = false,
                        Message = "Could Not Delete",
                    };
                }
                var DeletedProduct= await _ProductRepository.DeleteAsync(product);
                return new Result<bool>
                {
                    Success = true,
                    Message = DeletedProduct ? "Success" : "Failed to Delete Product",
                    Data= DeletedProduct
                };
            }
            catch (Exception ex)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = ex.InnerException.Message,
                };
            }
        }
    }
}
