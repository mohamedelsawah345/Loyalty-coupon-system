using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.Customer
{
    public class UpdateCustomerViewModel
    {
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name must contain only letters, numbers, and spaces.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Account Number is required.")]
        [StringLength(20, ErrorMessage = "Account Number must be less than 20 digits.")]
        public string Code { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Governate must contain only letters and spaces.")]
        [Required(ErrorMessage = "Governate is required.")]
        public string? Governate { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }

        [Phone(ErrorMessage = "Please provide a valid phone number.")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Phone number must follow the format 01xxxxxxxxx.")]
        public string? PhoneNumber { get; set; }
        public int? TechnicianID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } 

    }

}
