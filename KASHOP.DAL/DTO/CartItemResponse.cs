using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO
{
    public class CartItemResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string MainImage { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Count;
        public int Count { get; set; }
    }
}
