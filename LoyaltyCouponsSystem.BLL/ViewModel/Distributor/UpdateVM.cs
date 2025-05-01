using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyCouponsSystem.BLL.ViewModel.Distributor
{
    public class UpdateVM
    {
        public int DistributorID { get; set; }

        [Required(ErrorMessage = "Account Number is required.")]
        [StringLength(20, ErrorMessage = "Account Number must be less than 20 digits.")]
        public string Code { get; set; }

        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Primary Phone Number")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Phone number must follow the format 01xxxxxxxxx.")]
        public string PhoneNumber1 { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Required(ErrorMessage = "Governate is required.")]
        public string SelectedGovernate { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string SelectedCity { get; set; }
        public List<SelectListItem> Governates { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Cities { get; set; } = new List<SelectListItem>();
        public List<string>? SelectedCustomerCodes { get; set; }
        public List<string>? SelectedCustomerNames { get; set; }
        public List<CustomerViewModel>? AvailableCustomers { get; set; }
        public List<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
        public bool IsActive { get; set; }
    }
}
