using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity.Permission
{
    public class UserPermission
    {
        public string UserId { get; set; }  // Foreign Key to User
        public ApplicationUser User { get; set; }

        public int PermissionId { get; set; }  // Foreign Key to Permission
        public Permission Permission { get; set; }
    }

}
