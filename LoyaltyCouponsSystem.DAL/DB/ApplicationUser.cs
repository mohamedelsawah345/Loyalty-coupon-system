using LoyaltyCouponsSystem.DAL.Entity;
using LoyaltyCouponsSystem.DAL.Entity.Permission;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.DB
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }         // This will also be used as the UserName.
        public string? Role { get; set; }
        public string? PhoneNumber { get; set; }
        public string? OptionalPhoneNumber { get; set; }
        public string? Governorate { get; set; }
        public string? City { get; set; }
        public string? NationalID { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string? Imagepath { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual Representative Representative { get; set; }
        public bool IsActive { get; set; } = true; // Default to active
        public ICollection<TechnicianUser> TechnicianUsers { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public ICollection<UserPermission> UserPermissions { get; set; }
        public ICollection<ApproveRecievedCoupons> ApproveRecievedCoupons { get; set; }

    }
}
