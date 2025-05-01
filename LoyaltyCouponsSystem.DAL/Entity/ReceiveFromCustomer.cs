using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class ReceiveFromCustomer
    {
        public int Id { get; set; } // Primary Key
        public string CustomerCode { get; set; } // Changed to string
        public string? DistributorCode { get; set; } // Changed to string
        public string TechnicianCode { get; set; } // Changed to string
        public long CouponReceiptNumber { get; set; } // Numeric
        public DateTime TransactionDate { get; set; }
        public int? GovernorateId { get; set; }
        public Governorate? Governorates { get; set; }
        public Area? Areas { get; set; }
        public int? AreaId { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Distributor? Distributor { get; set; }
        public virtual Technician? Technician { get; set; }
    }
}
