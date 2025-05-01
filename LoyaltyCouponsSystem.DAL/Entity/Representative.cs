using LoyaltyCouponsSystem.DAL.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class Representative
    {
        [Key]
        public string ApplicationUserId { get; set; } 
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string NationalId { get; set; }
        public string PhoneNumber { get; set; } 
        public string OptionalPhoneNumber { get; set; } 
        public string Governate { get; set; } 
        public string City { get; set; } 
        public string ApprovalStatus { get; set; } 
        public ICollection<Coupon> Coupons { get; set; }
    }
}
