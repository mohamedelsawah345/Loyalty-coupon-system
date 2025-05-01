using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity.Permission
{
    public class RolePermission
    {
        public string RoleId { get; set; }  // IdentityRole's Id
        public int PermissionId { get; set; } // Permission entity's Id
        public string? PermissionName { get; set; }

        public virtual IdentityRole Role { get; set; }  // Reference to IdentityRole
        public virtual Permission Permission { get; set; }  // Reference to Permission

    }
}
