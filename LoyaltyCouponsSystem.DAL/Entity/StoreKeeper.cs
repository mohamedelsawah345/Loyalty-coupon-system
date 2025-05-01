using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class StoreKeeper
    {
        public int StoreKeeperID { get; set; }
        public string NameAttribute { get; set; }
        public string ContactDetails { get; set; } 
        public ICollection<Coupon> Coupons { get; set; } 
    }
}
