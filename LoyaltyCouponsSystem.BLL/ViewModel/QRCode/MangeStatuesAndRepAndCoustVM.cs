using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.QRCode
{
    public class MangeStatuesAndRepAndCoustVM
    {
        public string FromSequence { get; set; }

        public string ToSequence { get; set; }

        public int? RepresentativeId { get; set; }
        public string RepresentativeName { get; set; }
        public int? TechnicianId { get; set; }
        public string TechnicianName { get; set; }

        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }


    }
}
