using LoyaltyCouponsSystem.DAL.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class Admin 
    {
        [Key]
        public string ApplicationUserId { get; set; } // Foreign key for ApplicationUser
        public virtual ApplicationUser ApplicationUser { get; set; }

        public ICollection<AuditLog> AuditLogs { get; set; }
    }
}
