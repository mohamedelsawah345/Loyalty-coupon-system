using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.QRCode
{
    public class CouponViewModel
    {
        public string SerialNumber { get; set; }
        public string TypeOfCoupone { get; set; }
        public string GovernorateName { get; set; }
        public string AreaName { get; set; }
        public decimal? Value { get; set; }
        public long? NumInYear { get; set; }
        public string Status { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string ScannedBy { get; set; }

       
        public string RepresentativeCode { get; set; }
        
        public string TechnicianCode { get; set; }
        
        public string CustomerCode { get; set; }

        public string DistubuterCode { get; set; }


    }
}
