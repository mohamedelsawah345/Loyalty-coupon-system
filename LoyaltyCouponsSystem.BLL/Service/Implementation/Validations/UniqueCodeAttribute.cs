using LoyaltyCouponsSystem.DAL.DB;
using LoyaltyCouponsSystem.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class UniqueCodeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
        var entityType = validationContext.ObjectType; // Get the type of the entity being validated
        string code = value?.ToString()?.Trim(); // Trim the code to remove leading/trailing spaces

        if (string.IsNullOrEmpty(code)) return ValidationResult.Success;

        if (entityType == typeof(Customer))
        {
            var existsCustomer = context.Customers.Any(c => EF.Functions.Like(c.Code, code));
            if (existsCustomer)
            {
                return new ValidationResult("The code is already in use for a customer.");
            }
        }
        else if (entityType == typeof(Distributor))
        {
            var existsDistributor = context.Distributors.Any(d => EF.Functions.Like(d.Code, code));
            if (existsDistributor)
            {
                return new ValidationResult("The code is already in use for a distributor.");
            }
        }
        else if (entityType == typeof(Technician))
        {
            var existsTechnician = context.Technicians.Any(t => EF.Functions.Like(t.Code, code));
            if (existsTechnician)
            {
                return new ValidationResult("The code is already in use for a technician.");
            }
        }

        return ValidationResult.Success;
    }
}
