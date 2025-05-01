using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class LogIssue
    {
        [Key]
        public int id { get; set; }

        public string?  Message { get; set; }

        public string? Code { get; set; }

    }
}
