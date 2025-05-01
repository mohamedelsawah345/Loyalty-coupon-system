using LoyaltyCouponsSystem.BLL.ViewModel.Customer;
using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LoyaltyCouponsSystem.BLL.ViewModel.Technician
{
    public class TechnicianViewModel
    {
        public int TechnicianID { get; set; }
        [Required(ErrorMessage = "Account Number is required.")]
        [StringLength(20, ErrorMessage = "Account Number must be less than 20 digits.")]
        [UniqueCode(ErrorMessage = "This account number is already in use.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string? NickName { get; set; }
        [Required(ErrorMessage = "National ID is required.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 digits long.")]
        [Display(Name = "National ID")]
        public string NationalID { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Phone number must follow the format 01xxxxxxxxx.")]
        public string PhoneNumber1 { get; set; }

        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Phone number must follow the format 01xxxxxxxxx.")]
        [Display(Name = "Secondary Phone Number")]
        public string? PhoneNumber2 { get; set; }
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Phone number must follow the format 01xxxxxxxxx.")]
        [Display(Name = "Tertiary Phone Number")]
        public string? PhoneNumber3 { get; set; }
        public List<string>? SelectedCustomerId { get; set; }
        public List<string>? SelectedCustomerNames { get; set; }
        public List<SelectListItem>? Customers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? Users { get; set; } = new List<SelectListItem>();
        public List<string>? SelectedUserCodes { get; set; }
        public List<string>? SelectedUserNames { get; set; }
        public List<SelectListItem> Governates { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Cities { get; set; } = new List<SelectListItem>();
        [Required(ErrorMessage = "Governate is required.")]
        public string SelectedGovernate { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string SelectedCity { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public List<CustomerViewModel>? ActiveCustomers { get; set; } = new List<CustomerViewModel>();
        public List<ApplicationUser>? AssignedRepresentatives { get; set; } = new();
        public List<CustomerViewModel>? UnassignedActiveCustomers { get; set; } = new List<CustomerViewModel>();  // New property
        public List<ApplicationUser>? UnassignedActiveUsers { get; set; } = new List<ApplicationUser>();  // New property

    }
}
