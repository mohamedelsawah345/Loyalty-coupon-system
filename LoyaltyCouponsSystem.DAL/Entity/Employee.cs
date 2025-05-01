using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; } 
        public string Role { get; set; } 
        public ICollection<AuditLog> AuditLogs { get; set; } 
    }
}
