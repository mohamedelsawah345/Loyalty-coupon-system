using LoyaltyCouponsSystem.DAL.DB;
using System.ComponentModel.DataAnnotations;

namespace LoyaltyCouponsSystem.DAL.Entity
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Code { get; set; }
        public ICollection<Distributor> Distributors { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        [MaxLength(50)]
        public string? Governate { get; set; }

        [MaxLength(50)]
        public string? City { get; set; }
        [MaxLength(11)]
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<DistributorCustomer> DistributorCustomers { get; set; }
        public ICollection<TechnicianCustomer> TechnicianCustomers { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
