using LoyaltyCouponsSystem.DAL.DB;
using System.ComponentModel.DataAnnotations;

public class ValidStartSequenceAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
        var startSequenceNumber = (int)value;

        // Retrieve the last EndSequence value, parse it client-side
        var lastEndSequence = context.Transactions
            .AsEnumerable()  // Forces client-side evaluation
            .Max(t => (int?)int.Parse(t.SequenceEnd)) ?? 0;

        // Display the last EndSequence value for debugging/logging purposes (if needed)
        Console.WriteLine($"Last EndSequence: {lastEndSequence}");

        if (startSequenceNumber <= lastEndSequence)
        {
            return new ValidationResult(
                $"Start sequence number must be greater than the last end sequence ({lastEndSequence}).",
                new[] { validationContext.MemberName });
        }

        return ValidationResult.Success;
    }
}
