using KASHOP.BLL.Services.Interfaces;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Enums;
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


        public async Task<Result<OrderDetailsResponse>> GetUserOrderDetailsAsync(int orderId, string userId)
        {
            var order = await _unitOfWork.OrderRepository.GetOne(
                filter: o => o.Id == orderId && o.UserId == userId);
            return Result<OrderDetailsResponse>.Ok(order.Adapt<OrderDetailsResponse>());
        }

        public Task<Result<List<OrderSummaryResponse>>> GetAllOrdersAsync(OrderStatusEnum? status)
        {
            var orders = _unitOfWork.OrderRepository.GetAllAsync(
                filter: o => o.Status == null || o.Status == status);

            var response = orders.Result.OrderByDescending(o => o.OrderDate).Adapt<List<OrderSummaryResponse>>();

            return Task.FromResult(Result<List<OrderSummaryResponse>>.Ok(response));
        }

        public async Task<Result<OrderDetailsResponse>> ChangeStatusAsync(int orderId ,OrderStatusEnum newStatus)
        {
            var order = await _unitOfWork.OrderRepository.GetOne(
                filter: o=> o.Id == orderId
                );
            if (order == null) return Result<OrderDetailsResponse>.Fail("Oreder not found!");

            if(order.Status == newStatus) return Result<OrderDetailsResponse>.Fail($"Oreder id already {newStatus}");

            if(order.PaymentMethod == PaymentMethodEnum.visa && order.PaymentStatus != PaymentStatusEnum.Paid)
            {
                return Result<OrderDetailsResponse>.Fail($"Order is not paid yet");
            }

            //pending => confirmed => preparing => deliverd

            bool IsAllowed = false;

            if(order.Status == OrderStatusEnum.Pending && newStatus == OrderStatusEnum.Confirmed)
            {
                IsAllowed = true;
            }
            if (order.Status == OrderStatusEnum.Confirmed && newStatus == OrderStatusEnum.Preparing)
            {
                IsAllowed = true;
            }
            if (order.Status == OrderStatusEnum.Preparing && newStatus == OrderStatusEnum.Delivered)
            {
                IsAllowed = true;
            }
            if(!IsAllowed) return Result<OrderDetailsResponse>.Fail($"Can't change status from {order.Status} to {newStatus}");

            order.Status = newStatus;

            if(newStatus == OrderStatusEnum.Delivered)
            {
                order.ShippedDate = DateTime.Now;
                if(order.PaymentMethod == PaymentMethodEnum.Cash)
                {
                    order.PaymentStatus = PaymentStatusEnum.Paid;
                    order.AmountPaid = order.TotalAmount;
                    order.PaidAt = DateTime.Now;
                }
            }

            await _unitOfWork.CompleteAsync();
            return Result<OrderDetailsResponse>.Ok(order.Adapt<OrderDetailsResponse>());

        }
    }
}
