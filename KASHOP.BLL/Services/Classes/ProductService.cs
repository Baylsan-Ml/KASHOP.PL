using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _FileService;

        public ProductService(IUnitOfWork unitOfWork, IFileService FileService)
        {

            _unitOfWork = unitOfWork;
            _FileService = FileService;
        }

        public async Task<Result<ProductResponse>> CreateProductAsync(ProductRequest request)
        {
           
                if (request.MainImage is null)
                {
                    return Result<ProductResponse>.Fail("Main image is required");
                }
                var UploadResult = await _FileService.UploadAsync(request.MainImage);
                
                if (!UploadResult.Success)
                {
                    return Result<ProductResponse>.Fail(UploadResult.Message);
                    //{
                    //    Success = false,
                    //    Message = UploadResult.Message,
                    //};
                }
                var product = request.Adapt<Product>();
                product.MainImage = UploadResult.Data.Url;
                product.MainImagePublicId = UploadResult.Data.PublicId;
            await _unitOfWork.ProductRepository.CreateAsync(product);
                await _unitOfWork.CompleteAsync();

            return Result<ProductResponse>.Ok(product.Adapt<ProductResponse>());
        }
        public async Task<Result<List<ProductResponse>>> GetAllProductsAsync()
        {
                var products = await _unitOfWork.ProductRepository.GetAllAsync(
                    new string[] { nameof(Product.Translations), nameof(Product.Category) });
                return Result<List<ProductResponse>>.Ok(products.Adapt<List<ProductResponse>>(), "Success!");

        }

        public async Task<Result<ProductResponse>> GetProductAsyns(Expression<Func<Product, bool>> filter)
        {
                var product = await _unitOfWork.ProductRepository.GetOne(filter, new string[] { nameof(Product.Translations), nameof(Product.Category) });
                if (product == null)
                {return Result<ProductResponse>.Fail("Product Not Found :(");}

                return Result<ProductResponse>.Ok(product.Adapt<ProductResponse>(), "Success!");
        }

        public async Task<Result<ProductResponse>> UpdateProductAsync(int id, ProductRequest request)
        {
                var product = request.Adapt<Product>();
                product.Id = id;
                var UpdatedProduct = await _unitOfWork.ProductRepository.UpdateAsync(product);
                return new Result<ProductResponse>
                {
                    Success = true,
                    Message = "Product Updated Successfully!",
                    Data = UpdatedProduct.Adapt<ProductResponse>()
                };
        }

       

        public async Task<Result<bool>> DeleteProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetOne(p => p.Id == id);
            if(product is null)
                return Result<bool>.Fail("Product Not Found :(");
            
            var deleteImageResult = await _FileService.Delete(product.MainImagePublicId);
            if(!deleteImageResult.Success)
                return Result<bool>.Fail(deleteImageResult.Message);
            
            _unitOfWork.ProductRepository.Delete(product);
            var affectedRows = await _unitOfWork.CompleteAsync();
            return affectedRows > 0? Result<bool>.Ok(true, "Product Deleted Successfully!") : Result<bool>.Fail("Failed to Delete Product");

        }
    }
}
