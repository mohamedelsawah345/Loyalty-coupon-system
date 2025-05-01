using LoyaltyCouponsSystem.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.QRCode
{
    public class HistoryTransactionGeneratedQRVM
    {
        public string FromSerialNumber { get; set; }

        public string ToSerialNumber { get; set; }

        public int NumberOfCoupones { get; set; }
        public string TypeOfCoupone { get; set; }

        public DateTime CreationDateTime { get; set; }


        public DateTime? ClosureDate { get; set; }
        public ICollection<Representative> Representatives { get; set; }
        public int? RepresentativeId { get; set; }
        //public ICollection<Technician> Technicians { get; set; }
        //public int? TechnicianId { get; set; }
        public ICollection<StoreKeeper> StoreKeepers { get; set; }

        public int? StorekeeperID { get; set; }

        public string? GeneratedBy { get; set; }

        public int Value { get; set; }

        public string  Governorate { get; set; }

        public string Area{ get; set; }


        public bool FlagToPrint { get; set; } = true;
    }
}
