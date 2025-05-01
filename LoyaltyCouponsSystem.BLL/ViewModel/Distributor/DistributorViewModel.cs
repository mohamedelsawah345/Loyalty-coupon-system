using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using LoyaltyCouponsSystem.DAL.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LoyaltyCouponsSystem.BLL.ViewModel.Distributor
{
    public class DistributorViewModel
    {
        public int DistributorID { get; set; }

        [Required(ErrorMessage = "Account Number is required.")]
        [StringLength(20, ErrorMessage = "Account Number must be less than 20 digits.")]
        [UniqueCode(ErrorMessage = "This account number is already in use.")]
        public string Code { get; set; }

        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
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
        public List<int>? SelectedCustomerIds { get; set; }
        public List<string>? SelectedCustomerNames { get; set; }
        public List<CustomerViewModel>? AvailableCustomers { get; set; }
        public List<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
        public bool IsActive { get; set; } = true;

    }
}
