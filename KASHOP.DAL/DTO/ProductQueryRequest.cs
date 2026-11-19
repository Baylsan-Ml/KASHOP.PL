using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO
{
    public class ProductQueryRequest : PaginationRequest
    {
        public int? CategotyId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
        public bool? Descending { get; set; } = false;
    }
}
