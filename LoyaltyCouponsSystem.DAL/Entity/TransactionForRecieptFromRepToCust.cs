using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class TransactionForRecieptFromRepToCust
    {

        [Key]
        public int ID { get; set; }

        public string CustomerCode { get; set; }

        public string TechnitionCode { get; set; }

        public string ReprsentitiveCode { get; set; }

        public string ExchangePermissionNumber { get; set; }


        public DateTime CreationDateTime { get; set; }= DateTime.Now;


        public ICollection<Representative> Representatives { get; set; }
        public int? RepresentativeId { get; set; }
        public ICollection<Technician> Technicians { get; set; }
        public int? TechnicianId { get; set; }
        public ICollection<StoreKeeper> StoreKeepers { get; set; }

        public int? StorekeeperID { get; set; }

        public string? GeneratedBy { get; set; }

        public string? GovernorateName { get; set; }

        public string? AreaName { get; set; }
        public Governorate? Governorates { get; set; }
        public Area? Areas { get; set; }



    }
}
