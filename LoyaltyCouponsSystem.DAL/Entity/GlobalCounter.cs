using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class GlobalCounter
    {
        public long MaxSerialNumber1 {  get; set; }
        public long MaxSerialNumber2 { get; set; }
        public long MaxSerialNumber3 { get; set; }
        public long MaxSerialNumber4 { get; set; }
        public long MaxSerialNumber5 { get; set; }
        public long MaxSerialNumber6 { get; set; }
        public long MaXNumberInYear { get; set; }
        [Key]
        public int Year { get; set; }

        public int YearNotId { get; set; }



    }
}
