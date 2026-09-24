using KASHOP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services.Interfaces
{
    public interface ICartService
    {
        Task<Result<bool>> AddToCart(string userId, CartItemRequest request);
    }
}
