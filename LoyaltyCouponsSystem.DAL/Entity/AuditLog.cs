using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class AuditLog
    {
        public int AuditLogID { get; set; }
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public int? EmployeeID { get; set; }
        public Employee? Employee { get; set; }
        public int? AdminID { get; set; }
        public Admin? Admin { get; set; }
    }
}
