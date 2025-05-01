using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class TechnicianCustomer
    {
        public int TechnicianId { get; set; }
        public Technician Technician { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
