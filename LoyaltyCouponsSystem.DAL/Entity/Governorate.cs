using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class Governorate
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Area> Areas { get; set; } // العلاقة One-to-Many

        public List<Coupon> CouponList { get; set; }
        public List<ReceiveFromCustomer> receiveFromCustomer { get; set; }
        public List<ApproveRecievedCoupons> ApproveRecievedCoupons { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
