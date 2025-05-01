using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.QRCode
{
    public class HistoryCouponViewModel
    {
        public long SerialNumber { get; set; }
        
        public string TypeOfCoupone { get; set; }
        public string GovernorateName { get; set; }
        public string AreaName { get; set; }
        public decimal? Value { get; set; }
        public long? NumInYear { get; set; }
        public string Status { get; set; }
        public DateTime? CreationDateTime { get; set; }
    }
}
