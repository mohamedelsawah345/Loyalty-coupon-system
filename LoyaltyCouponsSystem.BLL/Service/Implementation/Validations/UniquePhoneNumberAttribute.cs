using LoyaltyCouponsSystem.DAL.DB;
using System.ComponentModel.DataAnnotations;

public class UniquePhoneNumberAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
        string phoneNumber = value?.ToString();

        if (string.IsNullOrEmpty(phoneNumber)) return ValidationResult.Success;

        // Convert the phone numbers from int to string for comparison
        var existsCustomer = context.Customers.Any(c => c.PhoneNumber.ToString() == phoneNumber);
        var existsDistributor = context.Distributors.Any(d => d.PhoneNumber1.ToString() == phoneNumber);
        var existsTechnician = context.Technicians.Any(t => t.PhoneNumber1.ToString() == phoneNumber);

        if (existsCustomer || existsDistributor || existsTechnician)
        {
            return new ValidationResult("The phone number is already in use.");
        }

        return ValidationResult.Success;
    }
}
