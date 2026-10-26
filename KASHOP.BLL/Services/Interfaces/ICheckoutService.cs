using KASHOP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<Result<OrderResponse>> Checkout(string userId, CheckoutRequest request);
    }
}
