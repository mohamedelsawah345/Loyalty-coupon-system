using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
     public class QRScanLog
     {
        [Key]
        public int Id { get; set; }
        public string QR_ID { get; set; }
        public DateTime ScanTime { get; set; }
        public string ScanedBy { get; set; }
     

        public string? SerialNumber { get; set; }
        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
        public string? CouponId { get; set; }
        [ForeignKey("CouponId")]
        public Coupon? Coupon { get; set; }


        public string? ReceiptNumber { get; set; }



     }
}
