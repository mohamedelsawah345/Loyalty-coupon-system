using LoyaltyCouponsSystem.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class TechnicianUser
    {
        public int TechnicianId { get; set; }
        public Technician Technician { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
