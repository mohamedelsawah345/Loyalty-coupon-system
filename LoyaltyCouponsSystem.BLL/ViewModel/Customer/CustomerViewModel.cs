using System.ComponentModel.DataAnnotations;

namespace LoyaltyCouponsSystem.BLL.ViewModel.Customer
{
    public class CustomerViewModel
    {
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name must contain only letters, numbers, and spaces.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Account Number is required.")]
        [StringLength(20, ErrorMessage = "Account Number must be less than 20 digits.")]
        [UniqueCode(ErrorMessage = "This account number is already in use.")]
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
        public bool IsActive { get; set; } = true;
        // Override Equals and GetHashCode to allow proper comparison
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;

            var other = (CustomerViewModel)obj;
            return this.CustomerID == other.CustomerID; // Compare by CustomerID or other unique properties
        }

        public override int GetHashCode()
        {
            return CustomerID.GetHashCode(); // Ensure GetHashCode is consistent with Equals
        }
    }
}
