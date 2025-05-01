using LoyaltyCouponsSystem.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class ApproveRecievedCoupons
    {
        public int Id { get; set; }
        public int? GovernorateId { get; set; }
        public Governorate? Governorates { get; set; }
        public Area? Areas { get; set; }
        public int? AreaId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; }
        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
