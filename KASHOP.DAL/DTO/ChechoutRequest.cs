using KASHOP.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO
{
    public class CheckoutRequest
    {
        public int ChechoutId { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
