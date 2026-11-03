using KASHOP.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO
{
    public class ChangeOrderStatusRequest
    {
        public int Id { get; set; }
        public OrderStatusEnum Status { get; set; }
    }
}
