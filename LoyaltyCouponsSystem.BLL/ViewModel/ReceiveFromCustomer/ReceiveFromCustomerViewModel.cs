using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.ReceiveFromCustomer
{
    public class ReceiveFromCustomerViewModel
    {
        [Required(ErrorMessage = "Customer is required.")]
        public int CustomerId { get; set; }
        public int? DistributorId { get; set; }

        [Required(ErrorMessage = "Technician is required.")]
        public int TechnicianId { get; set; }

        [Required(ErrorMessage = "Governorate is required.")]
        public int GovernorateId { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public int? AreaId { get; set; }
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Coupon Receipt Number is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Coupon Receipt Number must be a valid number.")]
        public long CouponReceiptNumber { get; set; }
        public string? CustomerCodeAndName { get; set; }
        public string? DistributorCodeAndName { get; set; }
        public string? TechnicianCodeAndName { get; set; }
        public string? GovernorateName { get; set; } 
        public string? AreaName { get; set; } 
    }
}
