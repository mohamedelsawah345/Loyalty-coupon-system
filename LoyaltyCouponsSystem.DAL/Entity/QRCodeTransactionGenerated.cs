using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class QRCodeTransactionGenerated
    {
        [Key]
        public int ID {  get; set; }

        public string FromSerialNumber { get; set; }

        public string ToSerialNumber { get; set; }

        public int NumberOfCoupones { get; set; }
        public string TypeOfCoupone { get; set; }
        
        public DateTime CreationDateTime { get; set; } 


        public DateTime? ClosureDate { get; set; }
        public ICollection<Representative> Representatives { get; set; }
        public int? RepresentativeId { get; set; }
        public ICollection<Technician> Technicians { get; set; }
        public int? TechnicianId { get; set; }
        public ICollection<StoreKeeper> StoreKeepers { get; set; }

        public int? StorekeeperID { get; set; }

        public string? GeneratedBy { get; set; }

        public int Value { get; set; }

        public int? GovernorateID{ get; set; }
      
        public int? AreaId { get; set; }
        public Governorate? Governorates { get; set; }
        public Area? Areas { get; set; }

        public bool? FlagToPrint { get; set; }=true;





    }
}
