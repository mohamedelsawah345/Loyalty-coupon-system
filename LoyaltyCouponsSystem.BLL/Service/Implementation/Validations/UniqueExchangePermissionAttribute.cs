using LoyaltyCouponsSystem.DAL.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

public class UniqueExchangePermissionAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
        string exchangePermission = value?.ToString()?.Trim(); // Trim the permission code to remove leading/trailing spaces

        if (string.IsNullOrEmpty(exchangePermission)) return ValidationResult.Success;

        // Check if the exchange permission exists for any transaction
        var exists = context.Transactions.Any(t => t.ExchangePermission == exchangePermission);

        if (exists)
        {
            return new ValidationResult("The exchange permission must be unique.");
        }

        return ValidationResult.Success;
    }
}
