using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Classes
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> AddToCart(string userId, CartItemRequest request)
        {
            var product = await _unitOfWork.ProductRepository.GetOne(p => p.Id == request.ProductId);
            if(product is null)
                return Result<bool>.Fail("Product not found");
            
            if(request.Count <0)
                return Result<bool>.Fail("Count must be greater than 0");
            if(product.Quantity < request.Count)
                return Result<bool>.Fail("Not enough stock available");

            var existingCartItem = await _unitOfWork.CartRepository.GetOne(
                c => c.UserId == userId && c.ProductId == request.ProductId);

            if (existingCartItem is not null)
            {
                existingCartItem.Count += request.Count;
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = request.ProductId,
                    UserId = userId,
                    Count = request.Count
                };
                await _unitOfWork.CartRepository.CreateAsync(newItem);
            }
            await _unitOfWork.CompleteAsync();

            return Result<bool>.Ok(true);
        }

    }
}
