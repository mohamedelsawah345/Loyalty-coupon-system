using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class Distributor
    {
        public int DistributorID { get; set; }
        public string Name { get; set; }
        [MaxLength(11)]
        [Phone]
        public string PhoneNumber1 { get; set; }
        public string? Governate { get; set; }
        public string? City { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<DistributorCustomer> DistributorCustomers { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
