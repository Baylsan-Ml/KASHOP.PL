using KASHOP.DAL.DTO;
using KASHOP.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Result<List<OrderSummaryResponse>>> GetUserOrdersAsync(string userId);
        Task<Result<OrderDetailsResponse>> GetUserOrderDetailsAsync(int orderId, string userId);
        Task<Result<List<OrderSummaryResponse>>> GetAllOrdersAsync(OrderStatusEnum? status);

    }
}
