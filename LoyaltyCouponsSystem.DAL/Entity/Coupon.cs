using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class Coupon
    {



        [Key]
        public string CouponeId { get; private set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "يجب إدخال الرقم التسلسلي")]
        
        public string SerialNumber { get; set; }

        public string TypeOfCoupone { get; set; }
        public string Status { get; set; } = "Created";
        public DateTime CreationDateTime { get; set; } = DateTime.Now;
       
      
        public DateTime? ClosureDate { get; set; }
        public ICollection<Representative> ?Representatives { get; set; } 
        
        public string? RepresentativeCode { get; set; }

        public ICollection<Customer>? Customers { get; set; }
       
        public string? CustomerCode { get; set; }

        [JsonProperty(ReferenceLoopHandling = ReferenceLoopHandling.Ignore)]
        public QRScanLog? QRScanLog { get; set; }

        public int? QRScanLogId { get; set; }   
        public ICollection<Distributor>? Distributors { get; set; }

        public string? DistributorCode { get; set; }

        public ICollection<Technician>? Technicians { get; set; }
        
        public string? TechnicianCode { get; set; }
        public ICollection<StoreKeeper>? StoreKeepers { get; set; }
        public string? StoreKeeperCode { get; set; }

       

        public string? CreatedBy { get; set; }
        public string? ScannedBy { get; set; }

        public int Value { get; set; }

        public int? GovernorateId { get; set; }
        public Governorate? Governorates { get; set; }
        public Area? Areas { get; set; }
        public int? AreaId { get; set; }
        public long NumInYear { get; set; }
        public bool FlagToPrint { get; set; } = true;

       

    }
}
