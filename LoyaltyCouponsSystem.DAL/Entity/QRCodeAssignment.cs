using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class QRCodeAssignment
    {
        public int QRCodeAssignmentID { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int TechnicianID { get; set; }
        public Technician Technician { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}
