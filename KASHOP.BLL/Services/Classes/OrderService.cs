using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Classes
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<OrderSummaryResponse>>> GetUserOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync(
                filter: o => o.UserId == userId,
                includes: new[] {nameof(Order.OrderItems)}
                );
            var response = orders.OrderByDescending(o => o.OrderDate).Adapt<List<OrderSummaryResponse>>();

            return Result<List<OrderSummaryResponse>>.Ok(response);
        }


        public Task<Result<OrderDetailsResponse>> GetUserOrderDetailsAsync(int orderId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
