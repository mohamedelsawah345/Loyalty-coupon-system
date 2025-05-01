using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class Area
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public int GovernateId { get; set; }

        public Governorate Governorate { get; set; } // العلاقة مع Governorate

        public List<Coupon> CouponList { get; set; }

        public List<ReceiveFromCustomer> receiveFromCustomer { get; set; }
        public List<ApproveRecievedCoupons> ApproveRecievedCoupons { get; set; }
        public List<Transaction> Transactions { get; set; }


    }
}
