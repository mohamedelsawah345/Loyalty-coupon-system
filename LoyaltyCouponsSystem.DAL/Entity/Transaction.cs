using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public decimal PurchaseAmount { get; set; }
        public string? TransactionType { get; set; }
        public DateTime Timestamp { get; set; }     
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int? TechnicianID { get; set; } 
        public Technician? Technician { get; set; }
        public int DistributorID { get; set; } 
        public Distributor Distributor { get; set; }
        public string? CouponSort { get; set; } 
        public string CouponType { get; set; } 
        public long SequenceNumber { get; set; }
        public string ExchangePermission { get; set; }
        public string? CreatedBy { get; set; }
        public string SequenceStart { get; set; }
        public string SequenceEnd { get; set; }
        public int? GovernorateId { get; set; }
        public Governorate? Governorates { get; set; }
        public Area? Areas { get; set; }
        public int? AreaId { get; set; }
        public string? GovernorateName { get; set; }  // Not mapped to DB
        public string? AreaName { get; set; }  // Not mapped to DB
    }
}
